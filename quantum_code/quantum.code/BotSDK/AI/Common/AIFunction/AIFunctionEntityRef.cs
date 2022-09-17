namespace Quantum
{
	public unsafe abstract partial class AIFunctionEntityRef
	{
		public abstract EntityRef Execute(Frame frame, EntityRef entity);
	}

	[BotSDKHidden]
	[System.Serializable]
	public unsafe partial class DefaultAIFunctionEntityRef : AIFunctionEntityRef
	{
		public override EntityRef Execute(Frame frame, EntityRef entity)
		{
			return default(EntityRef);
		}
	}
}
