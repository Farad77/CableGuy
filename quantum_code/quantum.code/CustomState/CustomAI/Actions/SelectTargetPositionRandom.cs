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
            var randomPos = FindRandomPosition(f, e);

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