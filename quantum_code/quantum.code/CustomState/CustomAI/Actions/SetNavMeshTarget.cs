using System;
using System.Runtime.InteropServices;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  [AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
  public partial class SetNavMeshTarget : AIAction
  {
    public AIBlackboardValueKey CurrentTargetKey;

    public override unsafe void Update(Frame f, EntityRef e)
    {
      var bbComponent = f.Unsafe.GetPointer<AIBlackboardComponent>(e);
      var targetPosition = bbComponent->GetVector3(f, CurrentTargetKey.Key);

      var navMeshAgent = f.Unsafe.GetPointer<NavMeshPathfinder>(e);
           // Log.Debug("nombre de navmesh " + f.Map.NavMeshes.Count);
      var navMesh = f.Map.NavMeshes["NavMesh3"];
      navMeshAgent->SetTarget(f, targetPosition, navMesh);
    }
  }
}
