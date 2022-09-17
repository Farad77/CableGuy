using System.Linq;
using System.Management.Instrumentation;
using Photon.Deterministic;
using Quantum.Collections;

namespace Quantum
{
    public unsafe class HazardSystem : SystemSignalsOnly, 
        ISignalOnTrigger3D,
        ISignalOnTriggerExit3D
    {
        private static bool IsRelevant(Frame f, EntityRef entity, EntityRef other)
        {
            return f.Has<Hazard>(entity) && f.Has<Health>(other);
        }

        private static void DealDamage(Frame f, EntityRef target, EntityRef origin)
        {
            var hazard = f.Unsafe.GetPointer<Hazard>(origin);
            var health = f.Unsafe.GetPointer<Health>(target);

            var damageAmount = hazard->DamageAmount * f.DeltaTime; 
            var damage = new Damage()
            {
                Amount = damageAmount,
                IsDiscrete = true,
                Origin = origin,
                Target = target
            };

            var healthList = f.ResolveList(health->DamageToAbsorb);
            healthList.Add(damage);

            var d = f.ResolveDictionary(hazard->DamageDealt);

            if (d.TryGetValue(target, out var damageDealt))
            {
                damageAmount += damageDealt;
                d[target] = damageAmount;
            } else {
                d.Add(target, damageAmount);
            }

            if (damageAmount < hazard->DamageAmount) return;
            f.Events.OnDamageDealt(damageAmount, target);
            d[target] = FP._0;
        }
        
        public void OnTriggerExit3D(Frame frame, ExitInfo3D info)
        {
            if (!IsRelevant(frame, info.Entity, info.Other)) return;

            var d = frame.ResolveDictionary(frame.Unsafe.GetPointer<Hazard>(info.Entity)->DamageDealt);
            
            if (!d.TryGetValue(info.Other, out var damageDealt)) return;
            
            frame.Events.OnDamageDealt(damageDealt, info.Other);
            d.Remove(info.Other);
        }

        public void OnTrigger3D(Frame frame, TriggerInfo3D info)
        {
            if (!IsRelevant(frame, info.Entity, info.Other)) return;
            DealDamage(frame, info.Other, info.Entity);
        }
    }
}