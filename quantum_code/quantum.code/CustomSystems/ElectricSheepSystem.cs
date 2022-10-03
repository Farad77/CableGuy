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
    }

    public unsafe class ElectricSheepSystem : SystemMainThreadFilter<ElectricSheepFilter>
    {

        public override void Update(Frame f, ref ElectricSheepFilter filter)
        {

            var navmesh = f.Map.NavMeshes["NavMesh3"];
            filter.NavMeshPathfind->SetTarget(f, f.Unsafe.GetPointer<Transform3D>(filter.ElectricSheep->entityPlayerRefToFollow)->Position, navmesh);
            filter.ElectricSheep->cumulTime += f.DeltaTime * FP._8;
        }
    }
}
