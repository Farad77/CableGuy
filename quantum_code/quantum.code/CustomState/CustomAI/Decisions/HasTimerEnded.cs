using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public partial class HasTimerEnded : HFSMDecision
    {
        public AIBlackboardValueKey CurrentTimeKey;
        public override unsafe bool Decide(Frame f, EntityRef e)
        {
            var bbComponent = f.Unsafe.GetPointer<AIBlackboardComponent>(e);
            var currentTime = bbComponent->GetFP(f, CurrentTimeKey.Key);
            
            return currentTime <= FP._0;
        }
    }
}