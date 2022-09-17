using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public unsafe partial class SetTimerRandom : AIAction
    {
        public AIBlackboardValueKey CurrentTimeKey;

        public FP MinIdleTime;
        public FP MaxIdleTime;

        public override void Update(Frame f, EntityRef e)
        {
            var bb = f.Unsafe.GetPointer<AIBlackboardComponent>(e);
            var timer = GetTimer(f, e, bb);
            bb->Set(f, CurrentTimeKey.Key, timer);
        }
        
        private FP GetTimer(Frame f, EntityRef e, AIBlackboardComponent* bb)
        {
            var timer = bb->GetFP(f, CurrentTimeKey.Key);
            timer = f.RNG->Next(MinIdleTime, MaxIdleTime);

            return timer;
        }
    }
}