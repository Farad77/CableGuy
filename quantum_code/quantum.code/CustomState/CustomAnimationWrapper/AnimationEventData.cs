using System;
using Photon.Deterministic;
using Quantum.Inspector;

namespace Quantum
{
  [Serializable]
  public struct AnimationEventData
  {
    [HideInInspector] public FP Time;
  }
}
