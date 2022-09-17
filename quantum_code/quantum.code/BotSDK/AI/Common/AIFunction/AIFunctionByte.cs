namespace Quantum
{
	public unsafe abstract partial class AIFunctionByte
	{
		public abstract byte Execute(Frame frame, EntityRef entity);
	}

	[BotSDKHidden]
	[System.Serializable]
	public unsafe partial class DefaultAIFunctionByte : AIFunctionByte
	{
		public override byte Execute(Frame frame, EntityRef entity)
		{
			return 0;
		}
	}
}
