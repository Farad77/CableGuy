namespace Quantum
{
	public unsafe abstract partial class AIFunctionInt
	{
		public abstract int Execute(Frame frame, EntityRef entity);
	}

	[BotSDKHidden]
	[System.Serializable]
	public unsafe partial class DefaultAIFunctionInt : AIFunctionInt
	{
		public override int Execute(Frame frame, EntityRef entity)
		{
			return 0;
		}
	}
}
