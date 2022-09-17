namespace Quantum
{
	[System.Serializable]
	public unsafe partial class AIFunctionOR : AIFunctionBool
	{
		public AIParamBool ValueA;
		public AIParamBool ValueB;

		public override bool Execute(Frame frame, EntityRef entity)
		{
			return ValueA.ResolveFunction(frame, entity) || ValueB.ResolveFunction(frame, entity);
		}
	}
}
