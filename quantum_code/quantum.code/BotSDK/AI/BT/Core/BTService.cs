using Photon.Deterministic;
using System;

namespace Quantum
{
	public unsafe abstract partial class BTService
	{
		public FP IntervalInSec;

		[BotSDKHidden] public Int32 Id;

		public virtual void Init(Frame frame, BTAgent* agent, AIBlackboardComponent* blackboard)
		{
			var endTimesList = frame.ResolveList<FP>(agent->ServicesEndTimes);
			endTimesList.Add(0);
		}

		public void SetEndTime(Frame frame, BTAgent* agent)
		{
			var endTimesList = frame.ResolveList<FP>(agent->ServicesEndTimes);
			endTimesList[Id] = frame.BotSDKGameTime + IntervalInSec;
		}

		public FP GetEndTime(Frame frame, BTAgent* agent)
		{
			var endTime = frame.ResolveList(agent->ServicesEndTimes);
			return endTime[Id];
		}

		public virtual void RunUpdate(BTParams btParams)
		{
			var endTime = GetEndTime(btParams.Frame, btParams.Agent);
			if (btParams.Frame.BotSDKGameTime >= endTime)
			{
				OnUpdate(btParams);
				SetEndTime(btParams.Frame, btParams.Agent);
			}
		}

		public virtual void OnEnter(BTParams btParams)
		{
			SetEndTime(btParams.Frame, btParams.Agent);
		}

		/// <summary>
		/// Called whenever the Service is part of the current subtree
		/// and its waiting time is already over
		/// </summary>
		protected abstract void OnUpdate(BTParams btParams);

		public static void TickServices(BTParams btParams)
		{
			var activeServicesList = btParams.Frame.ResolveList<AssetRefBTService>(btParams.Agent->ActiveServices);

			for (int i = 0; i < activeServicesList.Count; i++)
			{
				var service = btParams.Frame.FindAsset<BTService>(activeServicesList[i].Id);
				try
				{
					service.RunUpdate(btParams);
				}
				catch (Exception e)
				{
					Log.Error("Exception in Behaviour Tree service '{0}' ({1}) - setting node status to Failure", service.GetType().ToString(), service.Guid);
					Log.Exception(e);
				}
			}
		}
	}
}
