using Photon.Deterministic;

namespace Quantum
{
    public unsafe struct  PlayerMovementFilter
    {
        public EntityRef EntityRef;
        public PlayerID* PlayerID;
        public Transform3D* Transform;
        public CharacterController3D* Kcc;
    }
    
    unsafe class MovementSystem : SystemMainThreadFilter<PlayerMovementFilter>,ISignalOnTriggerEnter3D
    {
        private static readonly FP ROTATION_SPEED_MULTIPLIER = FP._5;
        private bool reset = false;
        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
            Log.Debug("Trigger enter "+ f.Has<KillZone>(info.Entity)+ " "+ f.Has<PlayerID>(info.Other)+" "+info.Other.ToString());

            //TODO:rajouter check joueur
            if (f.Has<KillZone>(info.Entity)&& f.Has<PlayerID>(info.Other))
            {
                EntityRef player = info.Other;
                reset = true;
                var resetPos = f.Get<ResetPos>(player);
                resetPos.reset = true;
                f.Set(player, resetPos);
                Log.Debug("TRIGGER KILLZONE");
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
           // var resetable = f.Get<ResetPos>(filter.PlayerID->PlayerRef);
            
            if (reset)
            {
               // filter.Transform->Position = new FPVector3(0, 0, 0);
                reset = false;
            }
            if (input->MovementHorizontal == 0 &&
                input->MoveBack.WasPressed && input->MovementVertical < 0) {
                filter.Transform->Rotation *= FPQuaternion.AngleAxis( FP.Rad2Deg * FP.Rad_180, FPVector3.Up);
                filter.Kcc->Move(f, filter.EntityRef, FPVector3.Zero);
                return;
            }
            
            var inputVector = new FPVector3(input->MovementHorizontal, FP._0, input->MovementVertical);
            var movementVector = filter.Transform->Rotation * inputVector;
            
            var movementAcceleration = FPVector2.Dot(filter.Transform->Forward.XZ.Normalized, movementVector.XZ);
            var forwardVelocity = FPMath.Abs(movementAcceleration);

            var angle = FPVector2.Radians(filter.Transform->Forward.XZ.Normalized, movementVector.XZ);
            angle = angle > FP.Rad_90 ? (angle - FP.Pi) : angle;
            angle *= FP.Rad2Deg;
            angle = FPMath.Abs(angle);
            var rotationAcceleration = FPVector2.Dot(filter.Transform->Right.XZ.Normalized, movementVector.XZ);

            var rotation = angle * rotationAcceleration * ROTATION_SPEED_MULTIPLIER * f.DeltaTime;
            var viewOrientation = input->MovementHorizontal == FP._0 ?
                                    FPQuaternion.Identity :
                                    FPQuaternion.AngleAxis(rotation, FPVector3.Up);
            
            filter.Transform->Rotation *= viewOrientation;

            if (input->Jump.WasPressed)
            {
                f.Events.PlayerJump(filter.PlayerID->PlayerRef);
                filter.Kcc->Jump(f);
            }

            filter.Kcc->Move(f, filter.EntityRef, filter.Transform->Forward * forwardVelocity);
        }
    }
}
