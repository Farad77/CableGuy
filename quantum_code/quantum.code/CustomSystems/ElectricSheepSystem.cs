using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Deterministic;

namespace Quantum
{
    public unsafe struct ElectricSheepFilter
    {
        public EntityRef EntityRef;
        public Transform3D* Transform;
        public ElectricSheepID* ElectricSheep;
        //public NavMeshSteeringAgent* NavMeshSteeringAgent;
        public NavMeshPathfinder* NavMeshPathfind;
    }

    public unsafe class ElectricSheepSystem : SystemMainThreadFilter<ElectricSheepFilter>
    {


        //public override void OnInit(Frame f)
        //{
        //    base.OnInit(f);

            /*var entity = f.Create();
            f.Set(entity, new Transform3D() { Position = FPVector3.Zero, Rotation = FPQuaternion.Identity });
            var config = f.FindAsset<NavMeshAgentConfig>(NavMeshAgentConfig.DEFAULT_ID);
            var pathfinder = NavMeshPathfinder.Create(f, entity, config);*/
            //f.Set(entity, new ElectricSheep() { oldPos  = FPVector3.Zero, cumulTime  = FP._0, entityPlayerRefToFollow  = });
        //}



        public override void Update(Frame f, ref ElectricSheepFilter filter)
        {

            if (filter.ElectricSheep->cumulTime == 0)
            {
                //f.Set(entity, new Transform3D() { Position = FPVector3.Zero, Rotation = FPQuaternion.Identity });
                //var config = f.FindAsset<NavMeshAgentConfig>(NavMeshAgentConfig.DEFAULT_ID);
                //filter.ElectricSheep->pathFinder = NavMeshPathfinder.Create(f, filter.EntityRef, config);

                // find a random point to move to
                //FPVector3 originPos= f.Unsafe.GetPointer<Transform3D>(filter.ElectricSheep->entityPlayerRefToFollow)->Position; 
                //if (navmesh.FindRandomPointOnNavmesh(FPVector3.Zero, FP._3 * FP._2, f.RNG, *f.NavMeshRegionMask, out FPVector3 randomPoint))
                //{
                //    filter.ElectricSheep->oldPos = originPos + randomPoint;
                //}

                //f.Set(filter.EntityRef, filter.ElectricSheep->pathFinder);
                //f.Set(filter.EntityRef, new NavMeshSteeringAgent());
                //NavMeshAgentConfig navConf = new NavMeshAgentConfig();
                //filter.ElectricSheep->pathFinder = NavMeshPathfinder.Create(f, filter.EntityRef, navConf);
                //filter.ElectricSheep->pathFinder.SetConfig(f, filter.EntityRef, navConf);

                //Log.Debug("pfiou on fait comment un Start hein?...");

            }
            var navmesh = f.Map.NavMeshes["NavMesh3"];
            //filter.ElectricSheep->pathFinder.SetTarget(f, f.Unsafe.GetPointer<Transform3D>(filter.ElectricSheep->entityPlayerRefToFollow)->Position, navmesh);
            filter.NavMeshPathfind->SetTarget(f, f.Unsafe.GetPointer<Transform3D>(filter.ElectricSheep->entityPlayerRefToFollow)->Position, navmesh);
            filter.ElectricSheep->cumulTime += f.DeltaTime * FP._8;
        }
    }
}
