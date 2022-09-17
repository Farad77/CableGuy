using Photon.Deterministic;

namespace Quantum
{
    public partial struct Knockable
    {
        public unsafe void Apply(Frame f, Physics3D.Hit3D hit, FP knockbackForce, FPVector3 attackerPosition)
        {
            if (!f.Unsafe.TryGetPointer(hit.Entity, out PhysicsBody3D* targetBody)) return;
            if (!f.Unsafe.TryGetPointer(hit.Entity, out Transform3D* targetTransform)) return;

            var knockbackDirection = (targetTransform->Position - attackerPosition).Normalized;
            var knockbackVector = knockbackDirection * knockbackForce;
            targetBody->AddForce(knockbackVector, hit.Point);
        }
    }
}