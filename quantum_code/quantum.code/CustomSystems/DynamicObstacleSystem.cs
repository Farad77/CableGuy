using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum
{
    public unsafe struct ObstacleFilter
    {
        public DynamicObstacle* obstacle;
        public EntityRef EntityRef;
        public Transform3D* Transform;
       
        public CharacterController3D* Kcc;
    }
    unsafe class DynamicObstacleSystem : SystemMainThreadFilter<ObstacleFilter>
    {

        public override void OnInit(Frame f)
        {
            base.OnInit(f);
        }
        public override void Update(Frame f, ref ObstacleFilter filter)
        {
            var speed = filter.obstacle->speed;
            //filter.obstacle->speed = 8;

           // filter.obstacle->targetPos = filter.obstacle->pos1;
            if (filter.obstacle->targetPos == FPVector3.Zero)
            {
                //Log.Debug("CES EGAL");
                filter.obstacle->targetPos = filter.obstacle->pos1;
               // Log.Debug("target X " + filter.obstacle->targetPos.X);
            }
            //Log.Debug("distance " + FPVector3.Distance(filter.Transform->Position, filter.obstacle->pos1));
            //filter.obstacle->targetPos == filter.obstacle->pos1 &&
            if (filter.obstacle->targetPos.X == filter.obstacle->pos1.X&&
               FPVector3.Distance(filter.Transform->Position, filter.obstacle->pos1 )< 5)
            {
                filter.obstacle->targetPos = filter.obstacle->pos2;
            }
            if (filter.obstacle->targetPos.X == filter.obstacle->pos2.X &&
             FPVector3.Distance(filter.Transform->Position, filter.obstacle->pos2) < 5)
            {
                filter.obstacle->targetPos = filter.obstacle->pos1;
            }

            var direction = filter.obstacle->targetPos - filter.Transform->Position ;
            filter.Kcc->Move(f, filter.EntityRef,direction);
           
        }
    }
}
