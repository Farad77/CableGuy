using System;
using Photon.Deterministic;

namespace Quantum
{
    [Serializable]
    public partial class ItemPotionMana : ItemBase
    {
        public FP Value;
        public override void Use<T>(Frame f, EntityRef entityRef, T item)
        {
            var mana = f.Get<Mana>(entityRef);
            var manaPotion = f.FindAsset<ItemPotionMana>(item.Guid);
            mana.Amount += manaPotion.Value;
            f.Set(entityRef, mana);
            
            var inventory = f.Get<CharacterInventory>(entityRef);
            inventory.PotionsMana -= FP._1;
            f.Set(entityRef, inventory);
            
            f.Events.OnUsedManaPotion(manaPotion.Value, entityRef);
        }

        public override void OnPickUp<T>(Frame f, EntityRef entityRef, T item)
        {
            var inventory = f.Get<CharacterInventory>(entityRef);
            inventory.PotionsMana += FP._1;
            f.Set(entityRef, inventory);

            f.Events.OnPickUpManaPotion(inventory.PotionsMana, entityRef);
        }
    }
}