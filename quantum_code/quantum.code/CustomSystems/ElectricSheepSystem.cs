﻿using System;
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
        public NavMeshSteeringAgent* NavMeshSteeringAgent;
    }

    public unsafe class ElectricSheepSystem : SystemMainThreadFilter<ElectricSheepFilter>
    {


        public override void OnInit(Frame f)
        {
            base.OnInit(f);

            var entity = f.Create();
            f.Set(entity, new Transform3D() { Position = FPVector3.Zero, Rotation = FPQuaternion.Identity });
            var config = f.FindAsset<NavMeshAgentConfig>(NavMeshAgentConfig.DEFAULT_ID);
            var pathfinder = NavMeshPathfinder.Create(f, entity, config);

            // y a t il des navmesh dispo?
            Log.Debug("-----");
            //Log.Debug(f.Map.NavMeshes.Values.
            foreach(var nvm in f.Map.NavMeshes)
            {
                Log.Debug("key : " + nvm.Key + " velue : " + nvm.Value);
            }

            var result = f.Map.NavMeshes["NavMesh"];
            if (result != null)
            {
                Log.Debug("FOUND NavMesh!!!");
            }
            else Log.Debug("damnit NavMesh!!!");
            foreach (var a in f.DynamicAssetDB.Assets)
            {
                Log.Debug(a.ToString());
                if (a is NavMesh navmeshAsset)
                {
                    Log.Debug(navmeshAsset.Name);
                }
            }
            Log.Debug("-----");
            // find a random point to move to
            /*var navmesh = f.Map.NavMeshes["NavMesh"];
            if (navmesh.FindRandomPointOnNavmesh(FPVector3.Zero, FP._10, f.RNG, *f.NavMeshRegionMask, out FPVector3 randomPoint))
            {
                pathfinder.SetTarget(f, randomPoint, navmesh);
            }

            f.Set(entity, pathfinder);
            f.Set(entity, new NavMeshSteeringAgent());*/
        }



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
            filter.ElectricSheep->pathFinder.SetTarget(f, f.Unsafe.GetPointer<Transform3D>(filter.ElectricSheep->entityPlayerRefToFollow)->Position, navmesh);
            filter.ElectricSheep->cumulTime += f.DeltaTime * FP._8;
            //filter.Transform->Position = new FPVector3(filter.ElectricSheep->oldPos.X, filter.ElectricSheep->oldPos.Y + FPMath.Abs(FPMath.Sin(filter.ElectricSheep->cumulTime)) * FP._0_20, filter.ElectricSheep->oldPos.Z);
        }
    }
}
