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

        public float damageModifier = 1f;
        public float knockbackModifier = 1f;
        public float cooldownModifier = 1f;
        public float stunModifier = 1f;
        public float critModifier = 1f;
        [Space(10)]
        public List<Attack> attackList = new List<Attack>();
    }
}