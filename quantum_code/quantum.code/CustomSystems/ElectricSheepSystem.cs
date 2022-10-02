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
<<<<<<< Updated upstream
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

=======
        //public FPVector3 oldPos = FPVector3.Zero;
        //public FP cumulTime = 0;
>>>>>>> Stashed changes
        public override void Update(Frame f, ref ElectricSheepFilter filter)
        {
            if (filter.ElectricSheep->cumulTime == 0)
            {
                filter.ElectricSheep->oldPos = filter.Transform->Position; 
                //Log.Debug("pfiou on fait comment un Start hein?...");
<<<<<<< Updated upstream




=======
>>>>>>> Stashed changes
            }
            filter.ElectricSheep->cumulTime += f.DeltaTime * FP._8;
            filter.Transform->Position = new FPVector3(filter.ElectricSheep->oldPos.X, filter.ElectricSheep->oldPos.Y + FPMath.Abs(FPMath.Sin(filter.ElectricSheep->cumulTime)) * FP._0_20, filter.ElectricSheep->oldPos.Z);
        }
    }
}
