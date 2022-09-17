using System;

namespace Quantum
{
	/// <summary>
	/// The sequence task is similar to an and operation. It will return failure as soon as one of its child tasks return failure.
	/// If a child task returns success then it will sequentially run the next task. If all child tasks return success then it will return success.
	/// </summary>
	[Serializable]
	public unsafe partial class BTSequence : BTComposite
	{
		protected override BTStatus OnUpdate(BTParams btParams)
		{
			BTStatus status = BTStatus.Success;

			while (GetCurrentChild(btParams.Frame, btParams.Agent) < _childInstances.Length)
			{
				var currentChildId = GetCurrentChild(btParams.Frame, btParams.Agent);
				var child = _childInstances[currentChildId];
				status = child.RunUpdate(btParams);

				if (status == BTStatus.Abort)
				{
					if (btParams.Agent->IsAborting == true)
					{
						return BTStatus.Abort;
					}
					else
					{
						return BTStatus.Failure;
					}
				}

				if (status == BTStatus.Success)
				{
					SetCurrentChild(btParams.Frame, currentChildId + 1, btParams.Agent);
				}
				else
				{
					break;
				}
			}

			return status;
		}

		internal override void ChildCompletedRunning(BTParams btParams, BTStatus childResult)
		{
			if (childResult == BTStatus.Abort)
			{
				return;
			}

			if (childResult == BTStatus.Failure)
			{
				SetCurrentChild(btParams.Frame, _childInstances.Length, btParams.Agent);

				// If the child failed, then we already know that this sequence failed, so we can force it
				SetStatus(btParams.Frame, BTStatus.Failure, btParams.Agent);

				// Trigger the debugging callbacks
				BTManager.OnNodeFailure?.Invoke(btParams.Entity, Guid.Value);
				BTManager.OnNodeExit?.Invoke(btParams.Entity, Guid.Value);
			}
			else
			{
				var currentChild = GetCurrentChild(btParams.Frame, btParams.Agent);
				SetCurrentChild(btParams.Frame, currentChild + 1, btParams.Agent);
			}
		}
	}
}