using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public partial class ItemCoin : ItemBase
    {
        public FP Value;
        public override void Use<T>(Frame f, EntityRef entityRef, T item)
        {
            //TODO: Decide where to call "Spend" from.
        }

        public override void OnPickUp<T>(Frame f, EntityRef entityRef, T item)
        {
            var coin = f.FindAsset<ItemCoin>(item.Guid);
            var inventory = f.Get<CharacterInventory>(entityRef);
            inventory.Wallet += coin.Value;
            f.Set(entityRef, inventory);
            
            f.Events.OnPickUpCoins(inventory.Wallet, entityRef);
        }
    }
}