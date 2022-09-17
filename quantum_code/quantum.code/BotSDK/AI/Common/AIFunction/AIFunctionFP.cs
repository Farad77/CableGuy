using Photon.Deterministic;

namespace Quantum
{
	public abstract unsafe partial class AIFunctionFP
	{
		public abstract FP Execute(Frame frame, EntityRef entity);
	}

	[BotSDKHidden]
	[System.Serializable]
	public unsafe partial class DefaultAIFunctionFP : AIFunctionFP
	{
		public override FP Execute(Frame frame, EntityRef entity)
		{
			return FP._0;
		}
	}
}
