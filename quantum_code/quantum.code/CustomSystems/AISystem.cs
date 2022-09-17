using System.Diagnostics;
using Photon.Deterministic;

namespace Quantum
{
  public unsafe class AISystem : SystemMainThread
  {
    private const string GameInitDataPath = "Resources/DB/QAssets/DefaultGameInitData";
        public override void Update(Frame f)
        {
            foreach (var (entity, hfsmAgent) in f.Unsafe.GetComponentBlockIterator<HFSMAgent>())
            {
                if (hfsmAgent->Data.CurrentState.Id == default) {
                    InitializeBot(f, entity);
                }
                HFSMManager.Update(f, f.DeltaTime, &hfsmAgent->Data, entity);
            }
        }

        private static void InitializeBot(Frame f, EntityRef agentEntity)
        {
            var bbInitAsset = f.FindAsset<GameInitData>(f.Map.UserAsset.Id);
            if (bbInitAsset == null)
            {
                Log.Error("Please provide User Asset slot in the MapData does not contain a GameInitData Asset");
                return;
            }
            
            var hfsmAgent = f.Unsafe.GetPointer<HFSMAgent>(agentEntity);

            var blackboardComponent = f.Unsafe.GetPointer<AIBlackboardComponent>(agentEntity);
            var blackboardInitializer = f.FindAsset<AIBlackboardInitializer>(bbInitAsset.BlackboardInitializer.Id);
            
            AIBlackboardInitializer.InitializeBlackboard(f, blackboardComponent, blackboardInitializer);
            
            hfsmAgent = f.Unsafe.GetPointer<HFSMAgent>(agentEntity);
            var hfsmRoot = f.FindAsset<HFSMRoot>(bbInitAsset.HfsmRoot.Id);
            
            HFSMManager.Init(f, &hfsmAgent->Data, agentEntity, hfsmRoot);
        }
    }
}
