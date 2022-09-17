using System.Diagnostics;
using Photon.Deterministic;
using Quantum.Collections;

namespace Quantum
{
    public unsafe class HealthSystem : SystemMainThread
    {
        private const string AI_TAKE_DAMAGE_EVENT = "OnTakeDamage";

        public override void Update(Frame f)
        {
            foreach(var (entity, health) in f.Unsafe.GetComponentBlockIterator<Health>())
            {
                if (health->Amount <= FP._0) continue;

                var l = f.ResolveList(health->DamageToAbsorb);
                if (l.Count <= FP._0) continue;

                var damageReceived = CalculateDamage(f, entity, l);
                health->Amount -= damageReceived;
            }

            foreach(var (entity, health) in f.Unsafe.GetComponentBlockIterator<Health>())
            {
                if (health->Amount > FP._0) continue;
                f.Destroy(entity);
            }
        }

        private static FP CalculateDamage(Frame f, EntityRef entity, QList<Damage> damageToAbsorb)
        {
            var blockPercentage = FP._0;
            if (f.Unsafe.TryGetPointer<Shield>(entity, out var shield)) {
                if (shield->IsBlocking) {
                    blockPercentage = shield->BlockPercentage;
                }
            }
            
            var damage = FP._0;
            for (int i = 0; i < damageToAbsorb.Count; i++)
            {
                var additionalDamage = damageToAbsorb[i].Amount - damageToAbsorb[i].Amount * blockPercentage;
                damage += additionalDamage;

                if (damageToAbsorb[i].IsDiscrete) continue;
                
                f.Events.OnDamageDealt(additionalDamage, entity);
            }
            
            if (f.Unsafe.TryGetPointer<HFSMAgent>(entity, out var hfsmAgent)) {
                HFSMManager.TriggerEvent(f, &hfsmAgent->Data, entity, AI_TAKE_DAMAGE_EVENT);
            }
            
            damageToAbsorb.Clear();
            return damage;
        }
    }
}