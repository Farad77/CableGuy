using System;
using System.Collections.Generic;
using Photon.Deterministic;

namespace Quantum
{
  [Serializable]
  [AssetObjectConfig(GenerateLinkingScripts = true, GenerateAssetCreateMenu = false, GenerateAssetResetMethod = false)]
  public unsafe partial class SimpleDebugAction : AIAction
  {
    public string Message;

    public override unsafe void Update(Frame f, EntityRef e)
    {
      Log.Info(Message);
    }
  }
}