using Photon.Deterministic;
using System;

namespace Quantum
{
    public unsafe class ConsommationEnergieSystem : SystemMainThread
    {
       

           
       
        public override void OnInit(Frame f)
        {
            base.OnInit(f);
           
            //f.Global->CurrentPlayers = f.AllocateDictionary<Int32, EntityRef>();
        }
        public override void Update(Frame f)
        {
            if (f.Global->Pause == 1) return;
            // Log.Debug("Ca tick?!1");
            foreach (var (entity, prod) in f.Unsafe.GetComponentBlockIterator<Energie>())
            {
               // Log.Debug("Ca tick?!");
                if (prod->NextTick > FP._0) continue;
                Shoot(f, entity);
                // Log.Debug("On a un consommateur!");
                /* Regen(f, prod);
                 ResetTimer(f, prod);*/
                ResetTimer(f, prod);

            }
        }
       // private const string PROJECTILE_PROTOTYPE = "Resources/DB/EntityPrototypes/Bullet|EntityPrototype";

        private  void Shoot(in Frame f, in EntityRef entity)
        {
           

            var playerId = f.Get<PlayerID>(entity);
            var input = f.GetPlayerInput(playerId.PlayerRef);
            var weapon = f.Unsafe.GetPointer<Weapon>(entity);
            var aimEntity = weapon->aimEntity;

            var consom = f.Unsafe.GetPointer<Energie>(entity);
            playerClose(f, entity);
            if (consom->CurrentAmount <= 0) { 

                consom->CurrentAmount = 0;
                 return;

            }
            if (consom->CurrentAmount <= consom->MaxAmount)
            {
                consom->CurrentAmount -= weapon->EnergyCost;
               
                f.Events.OnRegenTick(consom->CurrentAmount, entity);
            }
            //Log.Debug("tir");
            var transform = f.Get<Transform3D>(entity);
            var aimTransform = f.Unsafe.GetPointer<Transform3D>(aimEntity);
            //Resources/DB/EntityPrototypes/Bullet|EntityPrototype

            //var proto = f.FindAsset<EntityPrototype>(PROJECTILE_PROTOTYPE);
           // Log.Debug(" weapon id  = " + weapon->WeaponSpec.Id);
            var weaponSpec = f.FindAsset<WeaponSpec>(weapon->WeaponSpec.Id);
              //  Log.Debug(" proj = " + weaponSpec.projectile);
            var proto = f.FindAsset<EntityPrototype>(weaponSpec.projectile);
            EntityRef bulletEntity = f.Create(proto);
          
            var t2 = f.Unsafe.GetPointer<Transform3D>(bulletEntity);

            aimTransform->Position = transform.Position;
            t2->Position = aimTransform->Position + aimTransform->Forward * FP._1_50;
            FPVector3 target = aim(f, transform, bulletEntity);
            if (target == default)
            {
                return;
            }
            var lookPos = target- t2->Position ;
            lookPos.Y = 0;
            lookPos = lookPos.Normalized;
            //Log.Debug("look: " + lookPos);
            //t2->Rotation = FPQuaternion.Euler(new FPVector3(0, -input->Angle, 0));
            t2->Rotation = FPQuaternion.LookRotation(lookPos);
            aimTransform->Rotation = FPQuaternion.LookRotation(lookPos);



        }

        public void playerClose(Frame f,EntityRef player)
        {
            var t = f.Get<Transform3D>(player);
            var hits = f.Physics3D.OverlapShape(t, Shape3D.CreateSphere(5));
          
            for (int i = 0; i < hits.Count; i++)
            {
                var hit = hits[i];
                // Log.Debug("HIT: " + hits.Count);
                if (hit.Entity != player && f.Has<PlayerID>(hit.Entity))
                {
                   
                    var t2 = f.Get<Transform3D>(hit.Entity);

                    if (FPVector3.Distance(t.Position, t2.Position) < 3)
                    {
                       // Log.Debug("je  touche: " + hit.Entity + " je suis " + player);
                        f.Events.PlayerBeginCharge(hit.Entity, t);
                    }
                    else
                    {
                        f.Events.PlayerEndCharge(hit.Entity, t);
                    }
                }
               

            }
            // Log.Debug("c'est pas un mob!");
           // return default;
        }
        public FPVector3 aim(Frame f,Transform3D origin,EntityRef me)
        {
            var hits = f.Physics3D.OverlapShape(origin, Shape3D.CreateSphere(5));
            EntityRef targetEntity = default;
            for (int i = 0; i < hits.Count; i++)
            {
                var hit = hits[i];
                // Log.Debug("HIT: " + hits.Count);
                
                
                    if (hit.Entity != me && f.Has<ElectricSheepID>(hit.Entity))
                {
                  
                    targetEntity = hit.Entity;
                   // Log.Debug("c'est un mob! " + targetEntity);
                    return f.Get<Transform3D>(targetEntity).Position;
                }

            }
           // Log.Debug("c'est pas un mob!");
            return default;
        }

         FP AngleBetweenTwoPoints(FPVector2 a, FPVector2 b)
        {
            return FP.FromFloat_UNSAFE((float)(Math.Atan2((float)(a.Y - b.Y), (float)(a.X - b.X)) * 57.29578F));
            
        }


        private static void ResetTimer(Frame f, Energie* prod)
        {
            prod->NextTick = 1;
                //f.RNG->Next(spawner->SpawnIntervalMin, spawner->SpawnIntervalMax);
        }

        public void OnTriggerExit3D(Frame f, ExitInfo3D info)
        {
            Log.Debug("Trigger exit recharge?");
            if (f.Has<ProducteurEnergie>(info.Entity) && f.Has<PlayerID>(info.Other))
            {
                var prod = f.Unsafe.GetPointer<ProducteurEnergie>(info.Entity);
                Log.Debug("Trigger exit recharge ");
                try
                {
                    var conso = f.ResolveList<EntityRef>(prod->consommateur);
                    conso.Remove(info.Other);
                }
                catch (Exception e)
                {
                    prod->consommateur = f.AllocateList<EntityRef>();

                    var conso = f.ResolveList<EntityRef>(prod->consommateur);
                    conso.Remove(info.Other);
                }
            }
        }
    }
}