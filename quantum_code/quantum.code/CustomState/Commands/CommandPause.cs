using Photon.Deterministic;

namespace Quantum
{
    public unsafe class CommandPause : DeterministicCommand
    {
        public long enemyPrototypeGUID;
        
        public override void Serialize(BitStream stream)
        {
            stream.Serialize(ref enemyPrototypeGUID);   
        }

        public void Execute(Frame f)
        {
            if (f.Global->Pause == 0) f.Global->Pause = 1;
            else if (f.Global->Pause == 1) f.Global->Pause = 0;
        }
    }
}