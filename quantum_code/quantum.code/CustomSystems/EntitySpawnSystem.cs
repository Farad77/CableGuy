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
        struct PlayerFilter
        {
            public EntityRef Entity;
            public PlayerID* PlayerId;
        }
        public List<EntityRef> PlayersList;
        public override void OnInit(Frame f)
        {
            var players = f.Unsafe.FilterStruct<PlayerFilter>();
            var playerStruct = default(PlayerFilter);

            while (players.Next(&playerStruct))
            {
                Log.Debug($"playerStruct.PlayerId->PlayerRef : {playerStruct.PlayerId->PlayerRef}");
            }

            Log.Debug("blablabla");
            foreach (var (entity, player) in f.Unsafe.GetComponentBlockIterator<PlayerID>())
            {
                Log.Debug($"Player entity player->PlayerRef._index : {player->PlayerRef._index}");
            }
        }
        public override void Update(Frame f)
        {
            ComponentFilterStruct<PlayerFilter> players;
            PlayerFilter playerStruct = default(PlayerFilter);


            bool bInit = false;
            foreach (var (entity, spawner) in f.Unsafe.GetComponentBlockIterator<EntitySpawner>())
            {
                if(!bInit)
                {
                    bInit = true;
                    players = f.Unsafe.FilterStruct<PlayerFilter>();
                    playerStruct = default(PlayerFilter);


                    players.Next(&playerStruct);
                    //while (players.Next(&playerStruct))
                    //{
                    //    Log.Debug($"playerStruct.PlayerId->PlayerRef : {playerStruct.PlayerId->PlayerRef}");
                    //}
                }
                //Log.Debug($"Spawner entity value {entity}");

                CheckSpawnedList(f, spawner);

                if (spawner->NextSpawn > FP._0) continue;

                //SpawnEntity(f, spawner, entity);
                SpawnEntity(f, spawner, playerStruct.Entity);
                ResetTimer(f, spawner);
            }
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
            var spawnerPosition = f.Unsafe.GetPointer<Transform3D>(spawnerEntityAsPlayer)->Position;

            var electricSheepID = f.Unsafe.GetPointer<ElectricSheepID>(spawnedEntity);
            electricSheepID->entityPlayerRefToFollow = spawnerEntityAsPlayer;

            var posX = spawnerPosition.X + f.RNG->Next(FP._0, spawner->SpawnRadius);
            var posZ = spawnerPosition.Z + f.RNG->Next(FP._0, spawner->SpawnRadius);
            
            entityTransform->Position = new FPVector3(posX, entityTransform->Position.Y, posZ);
            
            l.Add(spawnedEntity);
        }

        private static void ResetTimer(Frame f, EntitySpawner* spawner)
        {
            spawner->NextSpawn = f.RNG->Next(spawner->SpawnIntervalMin, spawner->SpawnIntervalMax);
        }
    }
}