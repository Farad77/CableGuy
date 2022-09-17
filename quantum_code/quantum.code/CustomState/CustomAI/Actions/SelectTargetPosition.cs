using System;
using System.Runtime.InteropServices;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  [AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
  public partial class SelectTargetPosition : AIAction
  {
    public AIBlackboardValueKey CurrentIDKey;
    public AIBlackboardValueKey CurrentTargetKey;

    public FPVector3 Position0;
    public FPVector3 Position1;

    public override unsafe void Update(Frame f, EntityRef e)
    {
      var bbComponent = f.Unsafe.GetPointer<AIBlackboardComponent>(e);

      var currentID = bbComponent->GetInteger(f, CurrentIDKey.Key);

      if(currentID % 2 == 0)
      {
        bbComponent->Set(f, CurrentTargetKey.Key, Position0);
      }

      if (currentID % 2 == 1)
      {
        bbComponent->Set(f, CurrentTargetKey.Key, Position1);
      }
    }
  }
}
