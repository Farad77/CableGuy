namespace Quantum
{
	public unsafe abstract partial class AIFunctionBool
	{
		public abstract bool Execute(Frame frame, EntityRef entity);
	}

	[BotSDKHidden]
	[System.Serializable]
	public unsafe partial class DefaultAIFunctionBool : AIFunctionBool
	{
		public override bool Execute(Frame frame, EntityRef entity)
		{
			return false;
		}
	}
}
