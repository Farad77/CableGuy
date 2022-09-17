using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public unsafe partial class SelectTargetFleeFromPredator : AIAction
    {
        public FP FleeCoefficient;
        public AIBlackboardValueKey CurrentTargetPosition;
        public AIBlackboardValueKey PredatorPosition;
        public override void Update(Frame f, EntityRef e)
        {
            var bb = f.Unsafe.GetPointer<AIBlackboardComponent>(e);
            var predatorPosition = bb->GetVector3(f, PredatorPosition.Key);

            var fleePosition = predatorPosition.XZ * -FleeCoefficient;
            fleePosition += f.Unsafe.GetPointer<Transform3D>(e)->Position.XZ;
            f.Map.NavMeshes["NavMesh"].ClampToGrid(ref fleePosition);
            
            bb->Set(f, CurrentTargetPosition.Key, fleePosition);
        }
    }
}