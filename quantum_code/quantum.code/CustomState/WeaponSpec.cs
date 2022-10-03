using Photon.Deterministic;
using System;

namespace Quantum
{
    public unsafe partial class WeaponSpec
    {
        public Shape3DConfig AttackShape;
        public LayerMask AttackLayers;
        public FP Damage;
        public FP KnockbackForce;
        public String projectile;
    }
}