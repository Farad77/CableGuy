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
        public NavMeshSteeringAgent* NavMeshSteeringAgent;
    }

    public unsafe class ElectricSheepSystem : SystemMainThreadFilter<ElectricSheepFilter>
    {
        //public FPVector3 oldPos = FPVector3.Zero;
        //public FP cumulTime = 0;
        public override void Update(Frame f, ref ElectricSheepFilter filter)
        {
            if (filter.ElectricSheep->cumulTime == 0)
            {
                filter.ElectricSheep->oldPos = filter.Transform->Position; 
                //Log.Debug("pfiou on fait comment un Start hein?...");
            }
            filter.ElectricSheep->cumulTime += f.DeltaTime * FP._8;
            filter.Transform->Position = new FPVector3(filter.ElectricSheep->oldPos.X, filter.ElectricSheep->oldPos.Y + FPMath.Abs(FPMath.Sin(filter.ElectricSheep->cumulTime)) * FP._0_20, filter.ElectricSheep->oldPos.Z);
        }
    }
}
