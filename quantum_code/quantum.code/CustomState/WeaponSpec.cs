using Photon.Deterministic;

namespace Quantum
{
    public unsafe partial class WeaponSpec
    {
        public Shape3DConfig AttackShape;
        public LayerMask AttackLayers;
        public FP Damage;
        public FP KnockbackForce;
    }
}