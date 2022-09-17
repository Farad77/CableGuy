namespace Quantum
{
	[System.Serializable]
	public unsafe partial class AIFunctionNOT : AIFunctionBool
	{
		public AIParamBool Value;

		public override bool Execute(Frame frame, EntityRef entity)
		{
			return !Value.ResolveFunction(frame, entity);
		}
	}
}
