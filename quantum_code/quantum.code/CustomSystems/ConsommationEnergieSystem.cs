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

        private static void Shoot(in Frame f, in EntityRef entity)
        {

            var playerId = f.Get<PlayerID>(entity);
            var input = f.GetPlayerInput(playerId.PlayerRef);
            var weapon = f.Unsafe.GetPointer<Weapon>(entity);

            var consom = f.Unsafe.GetPointer<Energie>(entity);
            if (consom->CurrentAmount <= 0) { 

                consom->CurrentAmount = 0;
                 return;

            }
            if (consom->CurrentAmount <= consom->MaxAmount)
            {
                consom->CurrentAmount -= weapon->EnergyCost;
               
                f.Events.OnRegenTick(consom->CurrentAmount, entity);
            }

            var transform = f.Get<Transform3D>(entity);

            //Resources/DB/EntityPrototypes/Bullet|EntityPrototype

            //var proto = f.FindAsset<EntityPrototype>(PROJECTILE_PROTOTYPE);
           // Log.Debug(" weapon id  = " + weapon->WeaponSpec.Id);
            var weaponSpec = f.FindAsset<WeaponSpec>(weapon->WeaponSpec.Id);
              //  Log.Debug(" proj = " + weaponSpec.projectile);
            var proto = f.FindAsset<EntityPrototype>(weaponSpec.projectile);
            EntityRef bulletEntity = f.Create(proto);
          
            var t2 = f.Unsafe.GetPointer<Transform3D>(bulletEntity);
          

            t2->Position = input->AimDirection + input->AimForward * FP._1_50;
            t2->Rotation = FPQuaternion.Euler(new FPVector3(0, -input->Angle, 0));
          


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