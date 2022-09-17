using Photon.Deterministic;
using System;

namespace Quantum
{
	public static unsafe class HFSMManager
	{
		public static Action<EntityRef, long, string> StateChanged;

		/// <summary>
		/// Initializes the HFSM, making the current state to be equals the initial state
		/// </summary>
		public static unsafe void Init(Frame frame, EntityRef entity, HFSMRoot root)
		{
			if (frame.Unsafe.TryGetPointer(entity, out HFSMAgent* agent))
			{
				HFSMData* hfsmData = &agent->Data;
				Init(frame, hfsmData, entity, root);
			}
			else
			{
				Log.Error("[Bot SDK] Tried to initialize an entity which has no HfsmAgent component");
			}
		}

		/// <summary>
		/// Initializes the HFSM, making the current state to be equals the initial state
		/// </summary>
		public static unsafe void Init(Frame frame, HFSMData* hfsm, EntityRef entity, HFSMRoot root)
		{
			hfsm->Root = root;
			if (hfsm->Root.Equals(default) == false)
			{
				HFSMState initialState = frame.FindAsset<HFSMState>(root.InitialState.Id);
				ChangeState(initialState, frame, hfsm, entity, "");
			}
		}


		/// <summary>
		/// Update the state of the HFSM.
		/// </summary>
		/// <param name="deltaTime">Usually the current deltaTime so the HFSM accumulates the time stood on the current state</param>
		public static void Update(Frame frame, FP deltaTime, EntityRef entity)
		{
			if (frame.Unsafe.TryGetPointer(entity, out HFSMAgent* agent))
			{
				HFSMData* hfsmData = &agent->Data;
				Update(frame, deltaTime, hfsmData, entity);
			}
			else
			{
				Log.Error("[Bot SDK] Tried to update an entity which has no HFSMAgent component");
			}
		}

		/// <summary>
		/// Update the state of the HFSM.
		/// </summary>
		/// <param name="deltaTime">Usually the current deltaTime so the HFSM accumulates the time stood on the current state</param>
		public static void Update(Frame frame, FP deltaTime, HFSMData* hfsmData, EntityRef entity)
		{
			HFSMState curentState = frame.FindAsset<HFSMState>(hfsmData->CurrentState.Id);
			curentState.UpdateState(frame, deltaTime, hfsmData, entity);
		}


		/// <summary>
		/// Triggers an event if the target HFSM listens to it
		/// </summary>
		public static unsafe void TriggerEvent(Frame frame, EntityRef entity, string eventName)
		{
			if (frame.Unsafe.TryGetPointer(entity, out HFSMAgent* agent))
			{
				HFSMData* hfsmData = &agent->Data;
				TriggerEvent(frame, hfsmData, entity, eventName);
			}
			else
			{
				Log.Error("[Bot SDK] Tried to trigger an event to an entity which has no HFSMAgent component");
			}
		}

		/// <summary>
		/// Triggers an event if the target HFSM listens to it
		/// </summary>
		public static unsafe void TriggerEvent(Frame frame, HFSMData* hfsmData, EntityRef entity, string eventName)
		{
			Int32 eventInt = 0;

			HFSMRoot hfsmRoot = frame.FindAsset<HFSMRoot>(hfsmData->Root.Id);
			if (hfsmRoot.RegisteredEvents.TryGetValue(eventName, out eventInt))
			{
				if (hfsmData->CurrentState.Equals(default) == false)
				{
					HFSMState currentState = frame.FindAsset<HFSMState>(hfsmData->CurrentState.Id);
					currentState.Event(frame, hfsmData, entity, eventInt);
				}
			}
		}

		/// <summary>
		/// Triggers an event if the target HFSM listens to it
		/// </summary>
		public static unsafe void TriggerEventNumber(Frame frame, HFSMData* hfsmData, EntityRef entity, Int32 eventInt)
		{
			if (hfsmData->CurrentState.Equals(default) == false)
			{
				HFSMState currentState = frame.FindAsset<HFSMState>(hfsmData->CurrentState.Id);
				currentState.Event(frame, hfsmData, entity, eventInt);
			}
		}

		/// <summary>
		/// Executes the On Exit actions for the current state, then changes the current state
		/// </summary>
		internal static void ChangeState(HFSMState nextState, Frame frame, HFSMData* hfsmData, EntityRef entity, string transitionId)
		{
			Assert.Check(nextState != null, "Tried to change HFSM to a null state");

			HFSMState currentState = frame.FindAsset<HFSMState>(hfsmData->CurrentState.Id);
			currentState?.ExitState(nextState, frame, hfsmData, entity);
			hfsmData->CurrentState = nextState;

			if (frame.IsVerified == true && entity != default(EntityRef))
			{
				StateChanged?.Invoke(entity, hfsmData->CurrentState.Id.Value, transitionId);
			}

			nextState.EnterState(frame, hfsmData, entity);
		}
	}
}
