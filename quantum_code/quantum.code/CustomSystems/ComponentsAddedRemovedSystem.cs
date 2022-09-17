using System;
using Photon.Deterministic;

namespace Quantum
{
    public unsafe class ComponentsAddedRemovedSystem : SystemSignalsOnly, 
        ISignalOnComponentAdded<Weapon>, 
        ISignalOnComponentRemoved<Weapon>,
        ISignalOnComponentAdded<Health>,
        ISignalOnComponentRemoved<Health>,
        ISignalOnComponentAdded<PickUpSlot>,
        ISignalOnComponentAdded<EntitySpawner>,
        ISignalOnComponentRemoved<EntitySpawner>,
        ISignalOnComponentAdded<Hazard>,
        ISignalOnComponentRemoved<Hazard>
    {
        // Signals
        public void OnAdded(Frame f, EntityRef entity, Weapon* component)
        {
            component->AlreadyHit = f.AllocateList<EntityRef>();
        }

        public void OnRemoved(Frame f, EntityRef entity, Weapon* component)
        {
            f.FreeList(component->AlreadyHit);
        }
        
        public void OnAdded(Frame f, EntityRef entity, Health* component)
        {
            component->DamageToAbsorb = f.AllocateList<Damage>();
        }

        public void OnRemoved(Frame f, EntityRef entity, Health* component)
        {
            f.FreeList(component->DamageToAbsorb);
        }
        
        public void OnAdded(Frame f, EntityRef entity, PickUpSlot* component)
        {
            CallbackFlags flags = default;
            flags |= CallbackFlags.OnDynamicTriggerEnter;
            
            f.Physics3D.SetCallbacks(entity, flags);        
        }

        public void OnAdded(Frame f, EntityRef entity, EntitySpawner* component)
        {
            component->Spawned = f.AllocateList<EntityRef>();
        }
        
        public void OnRemoved(Frame f, EntityRef entity, EntitySpawner* component)
        {
            f.FreeList(component->Spawned);
            f.FreeList(component->EntityPrototypes);
        }

        public void OnAdded(Frame f, EntityRef entity, Hazard* component)
        {
            component->AlreadyDamaged = f.AllocateList<EntityTimer>(); 
        }

        public void OnRemoved(Frame f, EntityRef entity, Hazard* component)
        {
            f.FreeList(component->AlreadyDamaged);
        }
    }
}