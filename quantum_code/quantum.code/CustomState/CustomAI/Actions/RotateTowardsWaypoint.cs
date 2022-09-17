using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public unsafe partial class RotateTowardsWaypoint : AIAction
    {
        public FP RotationSpeed;

        public override void Update(Frame f, EntityRef e)
        {
            // TODO: refactor after Jonas merged his refactored branch.
            var entityTransform = f.Unsafe.GetPointer<Transform3D>(e);
            var navMeshAgent = f.Unsafe.GetPointer<NavMeshPathfinder>(e);
            var nextWaypoint = navMeshAgent->GetWaypoint(f, navMeshAgent->WaypointIndex);

            var truncatedEntityRotation = new FPVector2(entityTransform->Rotation.AsEuler.X, entityTransform->Rotation.AsEuler.Z);
            
            var targetRotation = FPQuaternion.FromToRotation(truncatedEntityRotation.XYO, nextWaypoint.XYZ);

            entityTransform->Rotation *= FPQuaternion.Slerp(entityTransform->Rotation, targetRotation, RotationSpeed * f.DeltaTime);
        }
    }
}