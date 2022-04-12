using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using AttackSystem;

namespace Inventory
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Oasis/WeaponItemItem", order = 0)]
    public class WeaponItem : InventoryItem 
    {
        private void Reset() 
        {
            category = InventoryCategories.Weapon;
        }

        public List<Attack> attackList = new List<Attack>();
    }
}