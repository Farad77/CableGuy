using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public unsafe partial class SelectTargetPositionRandom  : AIAction
    {
        public AIBlackboardValueKey CurrentTargetKey;
        public override void Update(Frame f, EntityRef e)
        {
            var bbComponent = f.Unsafe.GetPointer<AIBlackboardComponent>(e);
            //var randomPos = FindRandomPosition(f, e);
            var t = f.Get<Transform3D>(e);
            var hits = f.Physics3D.OverlapShape(t, Shape3D.CreateSphere(100));
            EntityRef targetEntity = default;
            for (int i = 0; i < hits.Count; i++)
            {
                var hit = hits[i];

                if (hit.Entity != e && f.Has<PlayerID>(hit.Entity))
                {
                    targetEntity = hit.Entity;
                }
            }
            var randomPos = f.Get<Transform3D>(targetEntity).Position;

            bbComponent->Set(f, CurrentTargetKey.Key, randomPos);
        }
        
        private FPVector3 FindRandomPosition(Frame f, EntityRef e)
        {
            var navMesh = f.Map.GetNavMesh("NavMesh");

            var currentPosition = f.Unsafe.GetPointer<Transform3D>(e)->Position;
            navMesh.FindRandomPointOnNavmesh(
                currentPosition, 
                navMesh.GridSizeX * navMesh.GridNodeSize / FP._4 - FP._1, 
                f.RNG, 
                NavMeshRegionMask.Default, 
                out var randomPosition
            );
            
            return randomPosition;
        }
    }
}