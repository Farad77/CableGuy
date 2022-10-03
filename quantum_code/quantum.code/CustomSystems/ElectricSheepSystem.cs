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
        public override void Update(Frame f, ref ElectricSheepFilter filter)
        {

            if (filter.ElectricSheep->cumulTime == 0)
            {
                //f.Set(entity, new Transform3D() { Position = FPVector3.Zero, Rotation = FPQuaternion.Identity });
                var config = f.FindAsset<NavMeshAgentConfig>(NavMeshAgentConfig.DEFAULT_ID);
                var pathfinder = NavMeshPathfinder.Create(f, filter.EntityRef, config);

                // find a random point to move to
                var navmesh = f.Map.NavMeshes["NavMesh3"];
                FPVector3 originPos= f.Unsafe.GetPointer<Transform3D>(filter.ElectricSheep->entityPlayerRefToFollow)->Position; 
                if (navmesh.FindRandomPointOnNavmesh(FPVector3.Zero, FP._3 * FP._2, f.RNG, *f.NavMeshRegionMask, out FPVector3 randomPoint))
                {
                    pathfinder.SetTarget(f, randomPoint, navmesh);
                    filter.ElectricSheep->oldPos = originPos + randomPoint;
                }

                f.Set(filter.EntityRef, pathfinder);
                f.Set(filter.EntityRef, new NavMeshSteeringAgent());


                //Log.Debug("pfiou on fait comment un Start hein?...");
            }
            filter.ElectricSheep->cumulTime += f.DeltaTime * FP._8;
            filter.Transform->Position = new FPVector3(filter.ElectricSheep->oldPos.X, filter.ElectricSheep->oldPos.Y + FPMath.Abs(FPMath.Sin(filter.ElectricSheep->cumulTime)) * FP._0_20, filter.ElectricSheep->oldPos.Z);
        }
    }
}
