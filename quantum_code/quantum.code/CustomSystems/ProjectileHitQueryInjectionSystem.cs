namespace Quantum
{
    public unsafe struct ProjectileFilter
    {
        public EntityRef EntityRef;
        public Transform3D* Transform;
        public Projectile* Component;
    }
    
    public unsafe class ProjectileHitQueryInjectionSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var projectileFilter = f.Unsafe.FilterStruct<ProjectileFilter>();
            var projectile = default(ProjectileFilter);
            
            while (projectileFilter.Next(&projectile))
            {
                projectile.Component->PathQueryIndex = f.Physics3D.AddRaycastQuery(
                    projectile.Transform->Position, 
                    projectile.Transform->Forward,
                    projectile.Component->Speed * f.DeltaTime);

                var spec = f.FindAsset<WeaponSpec>(projectile.Component->WeaponSpec.Id);
                
                projectile.Component->DamageZoneQueryIndex = f.Physics3D.AddOverlapShapeQuery(
                    projectile.Transform->Position, 
                    projectile.Transform->Rotation,
                    spec.AttackShape.CreateShape(f),
                    spec.AttackLayers);
            }
        }
    }
}