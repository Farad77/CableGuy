using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public unsafe partial class SenseEnvironment : AIAction
    {
        public FP SensingRadius;
        public FP ViewAngle;
        public string FleePlayerEventName;
        public AIBlackboardValueKey PredatorPosition;
        public override void Update(Frame f, EntityRef e)
        {
            var transform = f.Unsafe.GetPointer<Transform3D>(e);
            var currentPosition = transform->Position;
            var currentForward = transform->Forward;
            
            Log.Info(currentPosition);
            
            var hits = f.Physics3D.OverlapShape(
                currentPosition, FPQuaternion.Identity,
                Shape3D.CreateSphere(SensingRadius));

            FPVector2 nearestPredator = default;

            for (int i = 0; i < hits.Count; i++)
            {
                if (!f.Exists(hits[i].Entity)) continue;
                if (!f.Has<PlayerID>(hits[i].Entity)) continue;

                Log.Info("Found Player");
                
                var playerPosition = f.Unsafe.GetPointer<Transform3D>(hits[i].Entity)->Position;

                bool canSeePredator = ViewAngle / 2 >= FPVector3.Angle(currentForward, playerPosition);
                if (!canSeePredator) continue;
                
                if(nearestPredator == default) nearestPredator = playerPosition.XZ;

                nearestPredator =
                    FPVector2.DistanceSquared(currentPosition.XZ, nearestPredator) >
                    FPVector2.DistanceSquared(currentPosition.XZ, playerPosition.XZ)
                        ? playerPosition.XZ
                        : nearestPredator;
            }

            if (nearestPredator != default)
            {
                Flee(f, e, nearestPredator);
            }
        }

        private void Flee(Frame f, EntityRef e, FPVector2 predatorPosition)
        {
            Log.Info("Fleeing Player");
            
            var bb = f.Unsafe.GetPointer<AIBlackboardComponent>(e);
            bb->Set(f, PredatorPosition.Key, predatorPosition);
            var agent = f.Unsafe.GetPointer<HFSMAgent>(e);
            HFSMManager.TriggerEvent(f, &agent->Data, e, FleePlayerEventName);
        }
    }
}