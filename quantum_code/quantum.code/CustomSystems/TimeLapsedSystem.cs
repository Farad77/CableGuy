using Photon.Deterministic;

namespace Quantum
{ 
    public unsafe class TimeLapsedSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            TimeLapsedAnimation(f);
            PlayerInputAnimation(f);
            TimeLapsedSpawner(f);
            TimeLapsedEntityTimer(f);
        }

        private static void TimeLapsedAnimation(Frame f)
        {
            var deltaTime = f.DeltaTime;
            
            foreach (var animation in f.GetComponentIterator<QAnimationState>())
            {
                if (!animation.Component.IsAnimating) continue;

                var animationComponent = animation.Component;
                animationComponent.TimeLapsed += deltaTime;
                animationComponent.IsAnimating = animation.Component.TimeLapsed < f.FindAsset<ClipData>(animation.Component.AttackAnimation.Id).TotalLength;

                var entity = animation.Entity;
                f.Set(entity, animationComponent);
            }
        }
        
        private static void PlayerInputAnimation(Frame f)
        {
            var filter = f.Filter<PlayerID, QAnimationState>();

            while(filter.NextUnsafe(out var entity, out var playerId, out var animationState))
            {
                if(!f.GetPlayerInput(playerId->PlayerRef)->Attack.WasPressed) continue;

                animationState->IsAnimating = true;
                animationState->TimeLapsed = FP._0;
            }
        }

        private static void TimeLapsedSpawner(Frame f)
        {
            var deltaTime = f.DeltaTime;

            foreach (var spawner in f.GetComponentIterator<EntitySpawner>())
            {
                var spawnerComponent = spawner.Component;
                spawnerComponent.NextSpawn -= deltaTime;

                var entity = spawner.Entity;
                f.Set(entity, spawnerComponent);
                
            }
        }

        private static void TimeLapsedEntityTimer(Frame f)
        {
            var deltaTime = f.DeltaTime;

            foreach (var hazard in f.GetComponentIterator<Hazard>())
            {
                var hazardComponent = hazard.Component;
                var l = f.ResolveList(hazardComponent.AlreadyDamaged);

                if (l.Count <= FP._0) continue;

                for (int j = 0; j < l.Count; j++)
                {
                    l.GetPointer(j)->Timer -= deltaTime;

                    if (l.GetPointer(j)->Timer > FP._0) continue;
                    
                    l.RemoveAt(j);
                    j--;
                }

                var entity = hazard.Entity;
                f.Set(entity, hazardComponent);
            }
        }
    }
}