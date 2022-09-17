using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public partial class CoinFlip : HFSMDecision
    {
        public FP ValueRange;
        public FP TrueProbability;
        public override unsafe bool Decide(Frame f, EntityRef e)
        {
            var randomValue = f.RNG->Next(FP._0, ValueRange);
            return randomValue <= ValueRange * TrueProbability;
        }
    }
}