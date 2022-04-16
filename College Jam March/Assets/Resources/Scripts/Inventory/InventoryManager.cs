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
        public static InventoryManager instance;

        public GameObject pickupUIPrefab;
        public Canvas worldCanvas;
        [Space(10)]
        public InventoryCategories categories;
        public List<InventoryItem> inventory = new List<InventoryItem>();

        private void Awake() 
        {
            instance = this;
            for (int i=0; i < inventory.Count; i++)
            {
                inventory[i] = Instantiate(inventory[i]);
            }    
        }

        public void AddItem(InventoryItem item)
        {
            InventoryItem findItem = GetItem(item.name);
            if (findItem != null)
            {
                findItem.quantity += item.quantity;
            }
            else
            {
                inventory.Add(item);
            }
        }
        
        public void RemoveItem(string itemName)
        {
            InventoryItem findItem = GetItem(itemName);
            if (findItem != null)
            {
                findItem.quantity--;
                if (findItem.quantity <= 0)
                {
                    inventory.Remove(findItem);
                }
            }
        }

        public InventoryItem GetItem(string itemName)
        {
            InventoryItem result = null;
            foreach (InventoryItem item in inventory)
            {
                if (item.name == itemName)
                {
                    result = item;
                }
            }
            return result;
        }
    }
}