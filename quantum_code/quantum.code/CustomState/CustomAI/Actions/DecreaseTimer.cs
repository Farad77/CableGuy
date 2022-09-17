using System;

namespace Quantum
{
    [Serializable]
    public partial class DecreaseTimer : AIAction
    {
        public AIBlackboardValueKey CurrentTimerKey;
        public override void Update(Frame f, EntityRef e)
        {
            unsafe
            {
                var bb = f.Unsafe.GetPointer<AIBlackboardComponent>(e);
                var timer = bb->GetFP(f, CurrentTimerKey.Key);
                timer -= f.DeltaTime;
                
                bb->Set(f, CurrentTimerKey.Key, timer);
            }
        }
    }
}