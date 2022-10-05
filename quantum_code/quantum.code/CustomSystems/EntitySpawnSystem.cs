using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Deterministic;

namespace Quantum
{
    public unsafe class EntitySpawnSystem : SystemMainThread
    {
        public struct PlayerFilter
        {
            public EntityRef Entity;
            public PlayerID* PlayerId;
        }
        public List<EntityRef> PlayersList;
        public int IdNextPlayer = 0;
        public override void Update(Frame f)
        {
            if (f.Global->Pause == 1) return;

            ComponentFilterStruct<PlayerFilter> players;
            PlayerFilter playerStruct = default(PlayerFilter);

            players = f.Unsafe.FilterStruct<PlayerFilter>();
            playerStruct = default(PlayerFilter);
            if(!players.Next(&playerStruct)) return;
            

            EntityRef eEntPlRef = EntityRef.None;
            bool bInit = false;
            foreach (var (entity, spawner) in f.Unsafe.GetComponentBlockIterator<EntitySpawner>())
            {
                if(!bInit)
                {
                    bInit = true;

                    players = f.Unsafe.FilterStruct<PlayerFilter>();
                    PlayersList = new List<EntityRef>();
                    while (players.Next(&playerStruct))
                    {
                        Log.Debug($"found another playerStruct.PlayerId->PlayerRef : {playerStruct.PlayerId->PlayerRef}");
                        PlayersList.Add(playerStruct.Entity);
                    }


                    if (PlayersList.Count > 0)
                    {
                        if (IdNextPlayer >= PlayersList.Count) IdNextPlayer = 0; // just to be sure it didnt change
                        eEntPlRef = PlayersList[IdNextPlayer];
                    }
                    //else eEntPlRef = EntityRef.None;
                }
                //Log.Debug($"Spawner entity value {entity}");

                CheckSpawnedList(f, spawner);

                if (spawner->NextSpawn > FP._0) continue;

                SpawnEntity(f, spawner, eEntPlRef);
                ResetTimer(f, spawner);
            }
            IdNextPlayer++;
            if (IdNextPlayer >= PlayersList.Count) IdNextPlayer = 0; // regular loop

        }

        private static void CheckSpawnedList(Frame f, EntitySpawner* spawner)
        {
            var l = f.ResolveList(spawner->Spawned);
            for (int i = 0; i < l.Count; i++)
            {
                if (f.Exists(l[i])) continue;
                
                l.RemoveAt(i);
                i--;
            }
        }
        
        private static void SpawnEntity(Frame f, EntitySpawner* spawner, EntityRef spawnerEntityAsPlayer)
        {
            var l = f.ResolveList(spawner->Spawned);
            if (l.Count >= spawner->MaxSpawnAmount) return;

            var lp = f.ResolveList(spawner->EntityPrototypes);
            var spawnedEntity = f.Create(lp[f.RNG->Next(0, lp.Count)]);

            var entityTransform = f.Unsafe.GetPointer<Transform3D>(spawnedEntity);

            var electricSheepID = f.Unsafe.GetPointer<ElectricSheepID>(spawnedEntity);
            FPVector3 spawnerPosition = FPVector3.Zero;
            if (spawnerEntityAsPlayer != EntityRef.None)
            {
                spawnerPosition = f.Unsafe.GetPointer<Transform3D>(spawnerEntityAsPlayer)->Position;
                electricSheepID->entityPlayerRefToFollow = spawnerEntityAsPlayer;
            }
            else
            {
                Log.Debug("Couldn't find a Player... this mob will be its own target");
                spawnerPosition = FPVector3.Zero;
                electricSheepID->entityPlayerRefToFollow = spawnedEntity;
            }

            var posX = spawnerPosition.X - f.RNG->Next(-spawner->SpawnRadius, spawner->SpawnRadius);
            var posZ = spawnerPosition.Z - f.RNG->Next(-spawner->SpawnRadius, spawner->SpawnRadius);

            //entityTransform->Position = new FPVector3(posX, spawnerPosition.Y, posZ);
            entityTransform->Position = new FPVector3(posX, FP._0, posZ);

            electricSheepID->oldPos = entityTransform->Position; // just for init
            electricSheepID->cumulTime = FP._0; // just for init

            l.Add(spawnedEntity);
        }

        private static void ResetTimer(Frame f, EntitySpawner* spawner)
        {
            spawner->NextSpawn = f.RNG->Next(spawner->SpawnIntervalMin, spawner->SpawnIntervalMax);
        }
    }
}