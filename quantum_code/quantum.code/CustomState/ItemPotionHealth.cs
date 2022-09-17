using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public partial class ItemPotionHealth : ItemBase
    {
        public FP Value; 
        public override void Use<T>(Frame f, EntityRef entityRef, T item)
        {
            var health = f.Get<Health>(entityRef);
            var healthPotion = f.FindAsset<ItemPotionHealth>(item.Guid);
            health.Amount += healthPotion.Value;
            f.Set(entityRef, health);

            var inventory = f.Get<CharacterInventory>(entityRef);
            inventory.PotionsHealth -= FP._1;
            f.Set(entityRef, inventory);

            f.Events.OnUsedHealthPotion(healthPotion.Value, entityRef);
        }

        public override void OnPickUp<T>(Frame f, EntityRef entityRef, T item)
        {
            var inventory = f.Get<CharacterInventory>(entityRef);
            inventory.PotionsHealth += FP._1;
            f.Set(entityRef, inventory);

            f.Events.OnPickUpHealthPotion(inventory.PotionsHealth, entityRef);
        }
    }
}