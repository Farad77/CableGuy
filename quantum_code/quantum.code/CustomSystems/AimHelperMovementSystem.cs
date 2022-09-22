using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe struct AimHelperMovementFilter
    {
        public EntityRef EntityRef;
        public PlayerID* PlayerID;
        public Transform3D* Transform;
        public AimObject AimObject;

    }
    unsafe class AimHelperMovementSystem : SystemMainThreadFilter<AimHelperMovementFilter>
    {
        private static readonly FP ROTATION_SPEED_MULTIPLIER = FP._5;
        private bool reset = false;
       

        public override void Update(Frame f, ref AimHelperMovementFilter filter)
        {
            //Log.Debug("UPDATE PLAYER");
            var input = f.GetPlayerInput(filter.PlayerID->PlayerRef);
            
            //var aimobject = filter->

            var transformAim = filter.Transform;
            transformAim->Rotation = FPQuaternion.Euler(new FPVector3(0, -input->Angle, 0));
            
        }
        
    }

}
