using Photon.Deterministic;
using System;

namespace Quantum
{
	public unsafe partial struct BTAgent
	{
		// Used to setup info on the Unity debugger
		public string GetTreeAssetName(Frame frame) => frame.FindAsset<BTRoot>(Tree.Id).Path;

		public bool IsAborting => AbortNodeId != 0;

		public AIConfig GetConfig(Frame frame)
		{
			return frame.FindAsset<AIConfig>(Config.Id);
		}

		public void Initialize(Frame frame, EntityRef entityRef, BTAgent* agent, AssetRefBTNode tree, bool force = false)
		{
			if (this.Tree != default && force == false)
				return;

			// -- Cache the tree
			BTRoot treeAsset = frame.FindAsset<BTRoot>(tree.Id);
			this.Tree = treeAsset;

			// -- Allocate data
			// Success/Fail/Running
			NodesStatus = frame.AllocateList<Byte>(treeAsset.NodesAmount);

			// Next tick in which each service shall be updated
			ServicesEndTimes = frame.AllocateList<FP>(4);

			// Node data, such as FP for timers, Integers for IDs
			BTDataValues = frame.AllocateList<BTDataValue>(4);

			// The Services contained in the current sub-tree,
			// which should be updated considering its intervals
			ActiveServices = frame.AllocateList<AssetRefBTService>(4);

			// The Dynamic Composites contained in the current sub-tree,
			// which should be re-checked every tick
			DynamicComposites = frame.AllocateList<AssetRefBTComposite>(4);

			// -- Cache the Blackboard (if any)
			AIBlackboardComponent* blackboard = null;
			if (frame.Has<AIBlackboardComponent>(entityRef))
			{
				blackboard = frame.Unsafe.GetPointer<AIBlackboardComponent>(entityRef);
			}

			// -- Initialize the tree
			treeAsset.InitializeTree(frame, agent, blackboard);

			// -- Trigger the debugging event (mostly for the Unity side)
			BTManager.OnSetupDebugger?.Invoke(entityRef, treeAsset.Path);
		}

		public void Free(Frame frame)
		{
			Tree = default;
			frame.FreeList<Byte>(NodesStatus);
			frame.FreeList<FP>(ServicesEndTimes);
			frame.FreeList<BTDataValue>(BTDataValues);
			frame.FreeList<AssetRefBTService>(ActiveServices);
			frame.FreeList<AssetRefBTComposite>(DynamicComposites);
		}

		#region Int and FP Data
		// Getter / Setters of node FP and Int32 data
		public void AddFPData(Frame frame, FP fpValue)
		{
			var nodesDataList = frame.ResolveList<BTDataValue>(BTDataValues);
			BTDataValue newDataValue = new BTDataValue();
			*newDataValue.FPValue = fpValue;
			nodesDataList.Add(newDataValue);
		}

		public void AddIntData(Frame frame, Int32 intValue)
		{
			var nodesDataList = frame.ResolveList<BTDataValue>(BTDataValues);
			BTDataValue newDataValue = new BTDataValue();
			*newDataValue.IntValue = intValue;
			nodesDataList.Add(newDataValue);
		}

		public void SetFPData(Frame frame, FP value, Int32 index)
		{
			var nodesDataList = frame.ResolveList<BTDataValue>(BTDataValues);
			*nodesDataList.GetPointer(index)->FPValue = value;
		}

		public void SetIntData(Frame frame, Int32 value, Int32 index)
		{
			var nodesDataList = frame.ResolveList<BTDataValue>(BTDataValues);
			*nodesDataList.GetPointer(index)->IntValue = value;
		}

		public FP GetFPData(Frame frame, Int32 index)
		{
			var nodesDataList = frame.ResolveList<BTDataValue>(BTDataValues);
			return *nodesDataList.GetPointer(index)->FPValue;
		}

		public Int32 GetIntData(Frame frame, Int32 index)
		{
			var nodesDataList = frame.ResolveList<BTDataValue>(BTDataValues);
			return *nodesDataList.GetPointer(index)->IntValue;
		}
		#endregion

		public void Update(ref BTParams btParams)
		{
			if (btParams.Agent->Current == null)
			{
				btParams.Agent->Current = btParams.Agent->Tree;
			}

			RunDynamicComposites(btParams);

			BTNode node = btParams.Frame.FindAsset<BTNode>(btParams.Agent->Current.Id);
			UpdateSubtree(btParams, node);

			BTManager.ClearBTParams(btParams);
		}

		// We run the dynamic composites contained on the current sub-tree (if any)
		// If any of them result in "False", we abort the current sub-tree
		// and take the execution back to the topmost decorator so the agent can choose another path
		private void RunDynamicComposites(BTParams btParams)
		{
			var frame = btParams.Frame;
			var dynamicComposites = frame.ResolveList<AssetRefBTComposite>(DynamicComposites);

			for (int i = 0; i < dynamicComposites.Count; i++)
			{
				var compositeRef = dynamicComposites.GetPointer(i);
				var composite = frame.FindAsset<BTComposite>(compositeRef->Id);
				var dynamicResult = composite.OnDynamicRun(btParams);

				if (dynamicResult == false)
				{
					btParams.Agent->Current = composite.TopmostDecorator;
					dynamicComposites.Remove(*compositeRef);
					composite.OnReset(btParams);
					return;
				}
			}
		}

		private void UpdateSubtree(BTParams btParams, BTNode node, bool continuingAbort = false)
		{
			// Start updating the tree from the Current agent's node
			var result = node.RunUpdate(btParams, continuingAbort);

			// If the current node completes, go up in the tree until we hit a composite
			// Run that one. On success or fail continue going up.
			while (result != BTStatus.Running && node.Parent != null)
			{
				// As we are traversing the tree up, we allow nodes to remove any
				// data that is only needed locally
				node.OnExit(btParams);

				node = node.Parent;
				if (node.NodeType == BTNodeType.Composite)
				{
					((BTComposite)node).ChildCompletedRunning(btParams, result);
					result = node.RunUpdate(btParams, continuingAbort);
				}

				if (node.NodeType == BTNodeType.Decorator)
				{
					((BTDecorator)node).EvaluateAbortNode(btParams);
				}
			}

			BTService.TickServices(btParams);

			if (result != BTStatus.Running)
			{
				BTNode tree = btParams.Frame.FindAsset<BTNode>(btParams.Agent->Tree.Id);
				tree.OnReset(btParams);
				btParams.Agent->Current = btParams.Agent->Tree;
				BTManager.OnTreeCompleted?.Invoke(btParams.Entity);
				//Log.Info("Behaviour Tree completed with result '{0}'. It will re-start from '{1}'", result, btParams.Agent->Current.Id);
			}
		}

		public unsafe void AbortLowerPriority(BTParams btParams, BTNode node)
		{
			// Go up and find the next interesting node (composite or root)
			var topNode = node;
			while (
				topNode.NodeType != BTNodeType.Composite &&
				topNode.NodeType != BTNodeType.Root)
			{
				topNode = topNode.Parent;
			}

			if (topNode.NodeType == BTNodeType.Root)
			{
				return;
			}

			var nodeAsComposite = (topNode as BTComposite);
			nodeAsComposite.AbortNodes(btParams, nodeAsComposite.GetCurrentChild(btParams.Frame, btParams.Agent) + 1);
		}

		// Used to react to blackboard changes which are observed by Decorators
		// This is triggered by the Blackboard Entry itself, which has a list of Decorators that observes it
		public unsafe void OnDecoratorReaction(BTParams btParams, BTNode node, BTAbort abort, out bool abortSelf, out bool abortLowerPriotity)
		{
			abortSelf = false;
			abortLowerPriotity = false;

			var status = node.GetStatus(btParams.Frame, btParams.Agent);

			if (abort.IsSelf() && (status == BTStatus.Running || status == BTStatus.Inactive))
			{
				// Check condition again
				if (node.DryRun(btParams) == false)
				{
					abortSelf = true;
					node.OnAbort(btParams);
				}
			}

			if (abort.IsLowerPriority())
			{
				AbortLowerPriority(btParams, node);
				abortLowerPriotity = true;
			}
		}
	}
}