using System;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  [AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
  public partial class HasReachedPosition : HFSMDecision
  {
    public AIBlackboardValueKey CurrentTargetKey;

    public override unsafe bool Decide(Frame f, EntityRef e)
    {
      var bbComponent = f.Unsafe.GetPointer<AIBlackboardComponent>(e);
      var targetPosition = bbComponent->GetVector3(f, CurrentTargetKey.Key);

      var entityPosition = f.Get<Transform3D>(e).Position;

      return FPVector2.Distance(entityPosition.XZ, targetPosition.XZ) < FP._0_50;
    }
  }
}
