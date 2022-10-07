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
        public NavMeshPathfinder* NavMeshPathfind;
        public NavMeshSteeringAgent* agent;
    }

    public unsafe class ElectricSheepSystem : SystemMainThreadFilter<ElectricSheepFilter>
    {

        public override void Update(Frame f, ref ElectricSheepFilter filter)
        {
            if (filter.ElectricSheep->entityPlayerRefToFollow == default) return;
            if (f.Global->Pause == 1)
            {
                filter.NavMeshPathfind->Stop(f,filter.EntityRef,true);
                return;
            }
            
            var navmesh = f.Map.NavMeshes["NavMesh3"];
            filter.NavMeshPathfind->SetTarget(f, f.Unsafe.GetPointer<Transform3D>(filter.ElectricSheep->entityPlayerRefToFollow)->Position, navmesh);
            filter.ElectricSheep->cumulTime += f.DeltaTime * FP._8;
            
        }
    }
}
