using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [System.Flags]
    public enum InventoryCategories
    {
        Weapon = 1,
        Component = 2,
    }

    public class InventoryManager : MonoBehaviour 
    {
        public InventoryCategories categories;
        public List<InventoryItem> inventory = new List<InventoryItem>();

        private void Awake() 
        {
            for (int i=0; i < inventory.Count; i++)
            {
                inventory[i] = Instantiate(inventory[i]);
            }    
        }
    }
}