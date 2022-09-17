using Photon.Deterministic;

namespace Quantum
{
    public class CommandSpawnEnemy : DeterministicCommand
    {
        public long enemyPrototypeGUID;
        
        public override void Serialize(BitStream stream)
        {
            stream.Serialize(ref enemyPrototypeGUID);   
        }

        public void Execute(Frame f)
        {
            var enemyPrototype = f.FindAsset<EntityPrototype>(enemyPrototypeGUID);
            f.Create(enemyPrototype);
        }
    }
}