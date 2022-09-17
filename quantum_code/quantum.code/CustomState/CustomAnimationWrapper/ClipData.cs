﻿using Photon.Deterministic;
using Quantum.Inspector;

 namespace Quantum
{
  public enum MeleeStatus { AttackMissed, AttackHit, AttackBlocked, Blocking };

  public unsafe partial class ClipData
  {
    public string ClipName;

    [HideInInspector] public AnimationEventData StartEvent;
    [HideInInspector] public AnimationEventData EndEvent;
    [HideInInspector] public FP TotalLength;
  }
}
