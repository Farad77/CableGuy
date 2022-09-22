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
        public AimObject* AimObject;
       public EntityRef EntityRef;
         public Transform3D* Transform;
         public PlayerID* PlayerID;
       


    }
    unsafe class AimHelperMovementSystem : SystemMainThreadFilter<AimHelperMovementFilter>
    {
        private static readonly FP ROTATION_SPEED_MULTIPLIER = FP._5;
        private bool reset = false;
       

        public override void Update(Frame f, ref AimHelperMovementFilter filter)
        {
            Log.Debug("AIM FOR  PLAYER: ");
           /* var filtered = f.Filter<Transform3D, AimObject>();

            while (filtered.Next(out var e, out var t, out var b))
            {
                
                Log.Debug("AIM FOR  PLAYER: " + t);
            }*/

            // var input = f.GetPlayerInput(filter.PlayerID->PlayerRef);
            // Log.Debug("AIM FOR  PLAYER: " + filter.PlayerID->PlayerRef + " for entity " + filter.EntityRef.ToString() + "Angle: " + (-input->Angle));
             // if (filter.PlayerID->PlayerRef != 0) return;
             // if (!PlayerIsLocal(filter.PlayerID->PlayerRef)) return;
             //var aimobject = filter->

           /*  var transformAim = filter.Transform;
             transformAim->Rotation = FPQuaternion.Euler(new FPVector3(0, -input->Angle, 0));*/
            // transformAim->Position = f.Unsafe.GetPointer<Transform3D>(filter.PlayerID->PlayerRef)->Position;
        }

    }

}
