using Photon.Deterministic;
using System;

namespace Quantum
{
    public unsafe class ProductionEnergySystem : SystemMainThread, ISignalOnTriggerEnter3D, ISignalOnTriggerExit3D
    {
       

            public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
           
            if (f.Has<ProducteurEnergie>(info.Entity) && f.Has<PlayerID>(info.Other))
            {
                var prod = f.Unsafe.GetPointer<ProducteurEnergie>(info.Entity);
                //Log.Debug("Trigger enter recharge " + currentRegen);
                try
                {
                    var conso = f.ResolveList<EntityRef>(prod->consommateur);
                    conso.Add(info.Other);
                }
                catch(Exception e)
                {
                    prod->consommateur = f.AllocateList<EntityRef>();

                    var conso = f.ResolveList<EntityRef>(prod->consommateur);
                    conso.Add(info.Other);
                }

             

            }
        }
        //TODO: Remove on exit consommateur from list
        public override void OnInit(Frame f)
        {
            base.OnInit(f);
            //f.Global->CurrentPlayers = f.AllocateDictionary<Int32, EntityRef>();
        }
        public override void Update(Frame f)
        {
           // Log.Debug("Ca tick?!1");
            foreach (var (entity, prod) in f.Unsafe.GetComponentBlockIterator<ProducteurEnergie>())
            {
               // Log.Debug("Ca tick?!");
                if (prod->NextTick > FP._0) continue;
               // Log.Debug("On a un consommateur!");
                Regen(f, prod);
                ResetTimer(f, prod);
                //Log.Debug($"Spawner entity value {entity}");

                /* CheckSpawnedList(f, spawner);

                 if (spawner->NextSpawn > FP._0) continue;

                 SpawnEntity(f, spawner, entity);
                 ResetTimer(f, spawner);*/
            }
        }

        private static void Regen(Frame f, ProducteurEnergie* prod)
        {
            var l = f.ResolveList(prod->consommateur);
            for (int i = 0; i < l.Count; i++)
            {
                var consom = f.Unsafe.GetPointer<Energie>(l[i]);
                consom->CurrentAmount+=FP._1;
                Log.Debug("Ca regen! "+ consom->CurrentAmount);
                f.Events.OnRegenTick(consom->CurrentAmount, l[i]);
                /* if (f.Exists(l[i])) continue;

                 l.RemoveAt(i);
                 i--;*/
            }
        }
        
        private static void SpawnEntity(Frame f, EntitySpawner* spawner, EntityRef spawnerEntity)
        {
            var l = f.ResolveList(spawner->Spawned);
            if (l.Count >= spawner->MaxSpawnAmount) return;

            var lp = f.ResolveList(spawner->EntityPrototypes);
            var spawnedEntity = f.Create(lp[f.RNG->Next(0, lp.Count)]);

            var entityTransform = f.Unsafe.GetPointer<Transform3D>(spawnedEntity);
            var spawnerPosition = f.Unsafe.GetPointer<Transform3D>(spawnerEntity)->Position;

            var posX = spawnerPosition.X + f.RNG->Next(FP._0, spawner->SpawnRadius);
            var posZ = spawnerPosition.Z + f.RNG->Next(FP._0, spawner->SpawnRadius);
            
            entityTransform->Position = new FPVector3(posX, entityTransform->Position.Y, posZ);
            
            l.Add(spawnedEntity);
        }

        private static void ResetTimer(Frame f, ProducteurEnergie* prod)
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