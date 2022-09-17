namespace Quantum
{
	public partial struct HFSMAgent
	{
		// Used to setup info on the Unity debugger
		public string GetRootAssetName(Frame frame) => frame.FindAsset<HFSMRoot>(Data.Root.Id).Path;

		public AIConfig GetConfig(Frame frame)
		{
			return frame.FindAsset<AIConfig>(Config.Id);
		}
	}
}
