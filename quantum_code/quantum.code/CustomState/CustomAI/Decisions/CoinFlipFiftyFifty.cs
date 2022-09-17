using System;

namespace Quantum
{
    [Serializable]
    public partial class CoinFlipFiftyFifty : HFSMDecision
    {
        public override unsafe bool Decide(Frame f, EntityRef e)
        {
            bool flip = f.RNG->Next(0, 1) < 1;
            return flip;
        }
    }
}