using Quantum.Core;

namespace Quantum
{
    public class PickUpSystem : SystemSignalsOnly, ISignalOnTriggerEnter3D
    {
        public void OnTriggerEnter3D(Frame f, TriggerInfo3D info)
        {
           
           
            if (!IsPlayerPickUp(f, info.Entity, info.Other)) return;
            TriggerPickUp(f, info.Entity, info.Other);
        }

        private static bool IsPlayerPickUp(Frame f, EntityRef pickUp, EntityRef player)
        {
            return f.Has<PickUpSlot>(pickUp) && f.Has<PlayerID>(player);            
        }

        private static void TriggerPickUp(Frame f, EntityRef pickUp, EntityRef player)
        {
            var item = f.Get<PickUpSlot>(pickUp).Item;
            var itemAsset = f.FindAsset<ItemBase>(item.Id);
            itemAsset.OnPickUp(f, player, itemAsset);
            
            f.Destroy(pickUp);
        }
    }
}