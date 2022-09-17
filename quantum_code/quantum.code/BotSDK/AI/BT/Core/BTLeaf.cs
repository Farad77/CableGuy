using Photon.Deterministic;
using System;

namespace Quantum
{
	public unsafe abstract partial class BTLeaf : BTNode
	{
		public AssetRefBTService[] Services;
		protected BTService[] _serviceInstances;
		public BTService[] ServiceInstances
		{
			get
			{
				return _serviceInstances;
			}
		}

		public override BTNodeType NodeType
		{
			get
			{
				return BTNodeType.Leaf;
			}
		}

		public override unsafe void Init(Frame frame, AIBlackboardComponent* blackboard, BTAgent* agent)
		{
			base.Init(frame, blackboard, agent);

			for (int i = 0; i < Services.Length; i++)
			{
				BTService service = frame.FindAsset<BTService>(Services[i].Id);
				service.Init(frame, agent, blackboard);
			}
		}

		public override void OnEnterRunning(BTParams btParams)
		{
			var activeServicesList = btParams.Frame.ResolveList<AssetRefBTService>(btParams.Agent->ActiveServices);
			for (int i = 0; i < _serviceInstances.Length; i++)
			{
				_serviceInstances[i].OnEnter(btParams);
				activeServicesList.Add(Services[i]);
			}
		}

		public override void OnEnter(BTParams btParams)
		{
			base.OnEnter(btParams);
			BTManager.OnNodeEnter?.Invoke(btParams.Entity, Guid.Value);
		}

		public override void OnExit(BTParams btParams)
		{
			var activeServicesList = btParams.Frame.ResolveList<AssetRefBTService>(btParams.Agent->ActiveServices);
			for (Int32 i = 0; i < _serviceInstances.Length; i++)
			{
				activeServicesList.Remove(Services[i]);
			}

			BTManager.OnNodeExit?.Invoke(btParams.Entity, Guid.Value);
		}

		public override void OnReset(BTParams btParams)
		{
			base.OnReset(btParams);
			OnExit(btParams);
		}

		public override void Loaded(IResourceManager resourceManager, Native.Allocator allocator)
		{
			base.Loaded(resourceManager, allocator);

			// Cache the service assets links
			_serviceInstances = new BTService[Services.Length];
			for (int i = 0; i < Services.Length; i++)
			{
				_serviceInstances[i] = (BTService)resourceManager.GetAsset(Services[i].Id);
			}
		}
	}
}