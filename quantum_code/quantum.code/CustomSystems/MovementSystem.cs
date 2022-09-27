using Photon.Deterministic;
using System;

namespace Quantum
{
    public unsafe struct  PlayerMovementFilter
    {
        public EntityRef EntityRef;
        public PlayerID* PlayerID;
        public Transform3D* Transform;
        public CharacterController3D* Kcc;
        public AimObject* aim;
        
    }
 

    unsafe class MovementSystem : SystemMainThreadFilter<PlayerMovementFilter>,ISignalOnTriggerEnter3D
    {
        private static readonly FP ROTATION_SPEED_MULTIPLIER = FP._5;
        private bool reset = false;
        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
           // Log.Debug("Trigger enter "+ f.Has<KillZone>(info.Entity)+ " "+ f.Has<PlayerID>(info.Other)+" "+info.Other.ToString());

            //TODO:rajouter check joueur
            if (f.Has<KillZone>(info.Entity)&& f.Has<PlayerID>(info.Other))
            {
                EntityRef player = info.Other;
                reset = true;
                var resetPos = f.Get<ResetPos>(player);
                resetPos.reset = true;
                f.Set(player, resetPos);
               // Log.Debug("TRIGGER KILLZONE");
                var t = f.Get<Transform3D>(player);
                
                t.Position= new FPVector3(0, 0, 0);
                f.Set(player, t);
            }
        }
        public void ResetPlayerPos()
        {
           
        }

        public override void Update(Frame f, ref PlayerMovementFilter filter)
        {
            //Log.Debug("UPDATE PLAYER");
            var input = f.GetPlayerInput(filter.PlayerID->PlayerRef);

            var aim = filter.aim->Entity;
            var aimObject = f.Unsafe.GetPointer<Transform3D>(aim);
            
            if (input->MovementHorizontal < 0 && input->MovementVertical == 0)
            {
                filter.Transform->Rotation = FPQuaternion.AngleAxis(-90, FPVector3.Up);
            }
            if (input->MovementHorizontal < 0 && input->MovementVertical > 0)
            {
               filter.Transform->Rotation = FPQuaternion.AngleAxis(-45, FPVector3.Up);
            }
            if (input->MovementHorizontal > 0 && input->MovementVertical > 0)
            {
               filter.Transform->Rotation = FPQuaternion.AngleAxis(45, FPVector3.Up);
            }
            if (input->MovementHorizontal > 0 && input->MovementVertical == 0)
            {
               filter.Transform->Rotation = FPQuaternion.AngleAxis(90, FPVector3.Up);
            }
            if (input->MovementHorizontal > 0 && input->MovementVertical < 0)
            {
               filter.Transform->Rotation = FPQuaternion.AngleAxis(90+45, FPVector3.Up);
            }
            if (input->MovementHorizontal == 0 && input->MovementVertical > 0)
            {
                filter.Transform->Rotation = FPQuaternion.AngleAxis(0, FPVector3.Up);
            }
            if (input->MovementHorizontal == 0 && input->MovementVertical < 0)
            {
               filter.Transform->Rotation = FPQuaternion.AngleAxis(180, FPVector3.Up);
            }
            if (input->MovementHorizontal < 0 && input->MovementVertical < 0)
            {
               filter.Transform->Rotation = FPQuaternion.AngleAxis(180+45, FPVector3.Up);
            }
            var inputVector = new FPVector3(input->MovementHorizontal, FP._0, input->MovementVertical);
            var movementVector = filter.Transform->Rotation * inputVector;

            var movementAcceleration = FPVector2.Dot(filter.Transform->Forward.XZ.Normalized, movementVector.XZ);
            var forwardVelocity = FPMath.Abs(movementAcceleration);
            
            if (input->Jump.WasPressed)
            {
                f.Events.PlayerJump(filter.PlayerID->PlayerRef);
                filter.Kcc->Jump(f);
            }

            //THB
            if(input->Defend.WasPressed)
            {
                f.Events.PlayerHit(filter.PlayerID->PlayerRef);
            }

            filter.Kcc->Move(f, filter.EntityRef, inputVector );
            aimObject->Position = filter.Transform->Position;
          //  Log.Debug("player :" + filter.PlayerID->PlayerRef.ToString()+"vise avec "+ filter.aim->Entity + " angle="+ input->Angle);
            aimObject->Rotation= FPQuaternion.Euler(new FPVector3(0, -input->Angle, 0));

             // aimObject->Rotation = filter.Transform->Rotation;
            // FPQuaternion.Euler(new FPVector3(0, -input->Angle, 0));

        }

    }
   
}
