using System.Runtime.InteropServices;

namespace Quantum
{
	[StructLayout(LayoutKind.Auto)]
	public unsafe partial struct BTParams
	{
		private Frame _frame;
		private BTAgent* _agent;
		private EntityRef _entity;
		private AIBlackboardComponent* _blackboard;

		private BTParamsUser _userParams;

		public Frame Frame { get => _frame; }
		public BTAgent* Agent { get => _agent; }
		public EntityRef Entity { get => _entity; }
		public AIBlackboardComponent* Blackboard { get => _blackboard; }

		public BTParamsUser UserParams { get => _userParams; set => _userParams = value; }

		public void SetDefaultParams(Frame frame, BTAgent* agent, EntityRef entity, AIBlackboardComponent* blackboard = null)
		{
			_frame = frame;
			_agent = agent;
			_entity = entity;
			_blackboard = blackboard;
		}

		public void Reset(Frame frame)
		{
			_frame = default;
			_agent = default;
			_entity = default;
			_blackboard = default;

			_userParams = default;
		}
	}

	public partial struct BTParamsUser
	{
	}
}