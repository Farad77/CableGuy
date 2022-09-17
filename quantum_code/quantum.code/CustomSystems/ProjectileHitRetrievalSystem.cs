using Photon.Deterministic;

namespace Quantum
{
    // TODO: Refactor and place damage calculations into the AttackSystem.
    
    public unsafe class ProjectileHitRetrievalSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var projectileFilter = f.Unsafe.FilterStruct<ProjectileFilter>();
            var projectile = default(ProjectileFilter);

            while (projectileFilter.Next(&projectile))
            {
                var hitsOnTrajectory = f.Physics3D.GetQueryHits(projectile.Component->PathQueryIndex);
                if (hitsOnTrajectory.Count <= FP._0)
                {
                    projectile.Transform->Position = 
                        projectile.Transform->Rotation * 
                        projectile.Transform->Forward *
                        projectile.Component->Speed * f.DeltaTime;
                    continue;
                }

                var damageZoneHits = f.Physics3D.GetQueryHits(projectile.Component->DamageZoneQueryIndex);

                for (int i = 0; i < damageZoneHits.Count; i++)
                {
                    if (damageZoneHits[i].Entity == EntityRef.None) continue;
                    if (f.Unsafe.TryGetPointer(damageZoneHits[i].Entity, out Health* health) == false) continue;

                    var spec = f.FindAsset<WeaponSpec>(projectile.Component->WeaponSpec.Id);
                    var l = f.ResolveList(health->DamageToAbsorb);
                    l.Add(DealDamage(spec.Damage, damageZoneHits[i].Entity, projectile.EntityRef));
                }
            }
        }
        
        private static Damage DealDamage(FP damageAmount, EntityRef target, EntityRef origin)
        {
            var damage = new Damage()
            {
                Amount = damageAmount,
                Target = target,
                Origin = origin
            };

            return damage;
        }
    }
}