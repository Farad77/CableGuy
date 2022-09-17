using System;

namespace Quantum
{
    [Serializable]
    public unsafe partial class SetNavMeshAgent : AIAction
    {
        public AssetRefNavMeshAgentConfig NavMeshAgentConfig;
        
        public override void Update(Frame f, EntityRef e)
        {
            var navMeshPathFinder = f.Unsafe.GetPointer<NavMeshPathfinder>(e);
            if (navMeshPathFinder->ConfigId.Value == NavMeshAgentConfig.Id.Value) return;
            
            navMeshPathFinder->SetConfig(f, e, f.FindAsset<NavMeshAgentConfig>(NavMeshAgentConfig.Id));
        }
    }
}