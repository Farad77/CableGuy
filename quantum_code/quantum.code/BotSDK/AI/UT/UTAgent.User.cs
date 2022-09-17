namespace Quantum
{
	public partial struct UTAgent
	{
		// Used to setup info on the Unity debugger
		public string GetRootAssetName(Frame frame) => frame.FindAsset<UTRoot>(UtilityReasoner.UTRoot.Id).Path;

		public AIConfig GetConfig(Frame frame)
		{
			return frame.FindAsset<AIConfig>(Config.Id);
		}
	}
}
