using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public partial class IsWaypointInView : HFSMDecision
    {
        public FP InViewTolerance;
        public override unsafe bool Decide(Frame f, EntityRef e)
        {
            var entityTransform = f.Unsafe.GetPointer<Transform3D>(e);
            var navMeshAgent = f.Unsafe.GetPointer<NavMeshPathfinder>(e);
            var nextWaypoint = navMeshAgent->GetWaypoint(f, navMeshAgent->WaypointIndex);

            var angle = FPVector2.Angle(entityTransform->Forward.XZ, nextWaypoint.XZ);

            return angle <= InViewTolerance;
        }
    }
}