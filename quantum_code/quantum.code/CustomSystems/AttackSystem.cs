using System.Linq;
using Photon.Deterministic;

namespace Quantum
{
    public unsafe class AttackSystem : SystemMainThread
    {
        struct PlayerFilter
        {
            public EntityRef Entity;
            public PlayerID* PlayerId;
            public Weapon* Weapon;
        }
        public override void Update(Frame f)
        {
            PlayerInputAttack(f);
            PerformAttack(f);
        }

        private void PerformAttack(Frame f)
        {
            foreach (var animState in f.GetComponentIterator<QAnimationState>())
            {
                if (!animState.Component.IsAnimating) continue;
                
                var attackClip = f.FindAsset<ClipData>(animState.Component.AttackAnimation.Id);
                if (!IsAttacking(animState.Component.TimeLapsed, attackClip)) continue;

                var entity = animState.Entity;
                Attack(f, entity);
            }
        }

        private static void PlayerInputAttack(Frame f)
        {
            var players = f.Unsafe.FilterStruct<PlayerFilter>();
            var playerStruct = default(PlayerFilter);

            while (players.Next(&playerStruct))
            {
                if (!f.GetPlayerInput(playerStruct.PlayerId->PlayerRef)->Attack.WasPressed) continue;
                StartAttack(f, playerStruct);
            }

            players = f.Unsafe.FilterStruct<PlayerFilter>();
            while (players.Next(&playerStruct))
            {
                if (!f.GetPlayerInput(playerStruct.PlayerId->PlayerRef)->Action.WasPressed) continue;
                playerStruct.Weapon->IsEquipped = !playerStruct.Weapon->IsEquipped;
                f.Events.PlayerWeaponEquip(playerStruct.PlayerId->PlayerRef, playerStruct.Weapon->IsEquipped); 
            }
        }
        
        private static void StartAttack(in Frame f, PlayerFilter player)
        {
            if (!player.Weapon->IsEquipped)
            {
                player.Weapon->IsEquipped = true;
                f.Events.PlayerWeaponEquip(player.PlayerId->PlayerRef, true); 
            }

            f.Events.PlayerAttack(player.PlayerId->PlayerRef);

            var l = f.ResolveList(player.Weapon->AlreadyHit);
            l.Clear();
        }
        
        private static bool IsAttacking(FP normalizedAnimationTime, ClipData clip)
        {
            return normalizedAnimationTime >= clip.StartEvent.Time
                   && normalizedAnimationTime < clip.EndEvent.Time; 
        }
        
        private static void Attack(in Frame f, in EntityRef entity)
        {
            var transform = f.Unsafe.GetPointer<Transform3D>(entity);
            var weapon = f.Unsafe.GetPointer<Weapon>(entity);
            var weaponSpec = f.FindAsset<WeaponSpec>(weapon->WeaponSpec.Id);
            var attackShape = weaponSpec.AttackShape.CreateShape(f);

            var hits = f.Physics3D.OverlapShape(
                transform->Position,
                transform->Rotation,
                attackShape,
                weaponSpec.AttackLayers);
            
            if (hits.Count <= 0) return;

            var lEntities = f.ResolveList(weapon->AlreadyHit);

            for (int i = 0; i < hits.Count; i++)
            {
                if (hits[i].Entity == EntityRef.None) continue;
                if (lEntities.Contains(hits[i].Entity)) continue;
                if (f.Unsafe.TryGetPointer(hits[i].Entity, out Health* health))
                {
                    var damageToAbsorb = f.ResolveList(health->DamageToAbsorb);
                    damageToAbsorb.Add(DealDamage(weaponSpec.Damage, entity, hits[i].Entity));
                }
                
                if (f.Unsafe.TryGetPointer(hits[i].Entity, out Knockable* knockable))
                {
                    knockable->Apply(f, hits[i], weaponSpec.KnockbackForce, transform->Position);
                }

                lEntities.Add(hits[i].Entity);
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