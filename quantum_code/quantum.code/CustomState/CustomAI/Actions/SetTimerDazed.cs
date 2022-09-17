using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public unsafe partial class SetTimerDazed : AIAction
    {
        public AIBlackboardValueKey CurrentTimeKey;
        
        public AssetRefClipData DazedClipData;
        public override void Update(Frame f, EntityRef e)
        {
            var bb = f.Unsafe.GetPointer<AIBlackboardComponent>(e);
            var timer = GetTimer(f, e, bb);
            bb->Set(f, CurrentTimeKey.Key, timer);
        }
        
        private FP GetTimer(Frame f, EntityRef e, AIBlackboardComponent* bb)
        {
            return f.FindAsset<ClipData>(DazedClipData.Id).TotalLength;
        }
    }
}