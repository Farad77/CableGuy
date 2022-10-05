using Photon.Deterministic;
using System;

namespace Quantum
{
    public unsafe struct  AimMovementFilter
    {
        public EntityRef EntityRef;
        public PlayerID* PlayerID;
        public Transform3D* Transform;
     
        public AimObject* aim;
        
    }
 

    unsafe class AimMovementSystem : SystemMainThreadFilter<AimMovementFilter>
    {
        private static readonly FP ROTATION_SPEED_MULTIPLIER = FP._5;
        private bool reset = false;
       
        public void ResetPlayerPos()
        {
           
        }
        public override void OnInit(Frame f)
        {
            base.OnInit(f);

            //f.Global->Pause = 0;
        }
        public override void Update(Frame f, ref AimMovementFilter filter)
        {
            var input = f.GetPlayerInput(filter.PlayerID->PlayerRef);

            if (input->Defend.WasPressed && f.Global->Pause == 0) f.Global->Pause = 1;
            else if (input->Defend.WasPressed && f.Global->Pause == 1) f.Global->Pause = 0;
            if (f.Global->Pause == 1) return;
          
          
           var aim = filter.EntityRef;
            var aimObject = f.Unsafe.GetPointer<AimObject>(aim);
            var transform = f.Unsafe.GetPointer<Transform3D>(aim);
            if (aimObject->player == default) return;
            Log.Debug(" gameobject trouve " + filter.EntityRef.ToString() + " angle=" + -input->Angle);
            var transformToFollow = f.Unsafe.GetPointer<Transform3D>(aimObject->player);
            //aimObject->Position = filter.Transform->Position;
            transform->Position = transformToFollow->Position;
            //  Log.Debug("player :" + filter.PlayerID->PlayerRef.ToString()+"vise avec "+ filter.aim->Entity + " angle="+ input->Angle);
            transform->Rotation= FPQuaternion.Euler(new FPVector3(0, -input->Angle, 0));

             // aimObject->Rotation = filter.Transform->Rotation;
            // FPQuaternion.Euler(new FPVector3(0, -input->Angle, 0));

        }

    }
   
}
