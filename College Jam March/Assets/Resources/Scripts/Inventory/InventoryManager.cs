using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [System.Flags]
    public enum InventoryCategories
    {
        All = 0,
        Weapon = 1,
        Component = 2,
        Armour = 4,
        Consumables = 8
    }

    public class InventoryManager : MonoBehaviour 
    {
        public static InventoryManager instance;

        public GameObject pickupUIPrefab;
        public Canvas worldCanvas;
        [Space(10)]
        public GameObject categoryPrefab;
        public GameObject itemPrefab;
        public GameObject itemTooltipPrefab;
        [Space(10)]
        public InvFull inventoryMenu;
        public InventoryCategories categories;
        public List<InventoryItem> inventory = new List<InventoryItem>();

        private void Awake() 
        {
            instance = this;
            for (int i=0; i < inventory.Count; i++)
            {
                inventory[i] = Instantiate(inventory[i]);
            }

            //DEBUG
            inventoryMenu.UpdateMenu(inventory);
        }

        //Inventory Menu Methods

        public void B_ToggleInv()
        {
            if (inventoryMenu.gameObject.activeInHierarchy)
            {
                inventoryMenu.transform.parent.gameObject.SetActive(false);
                Player.PlayerRefs.instance.playerClass.UnlockMovement();
            }
            else
            {
                inventoryMenu.transform.parent.gameObject.SetActive(true);
                Player.PlayerRefs.instance.playerClass.LockMovement();
                inventoryMenu.UpdateMenu(inventory);
            }
        }

        //Inventory Methods

        public void AddItem(InventoryItem item)
        {
            InventoryItem findItem = GetItem(item.name.Split('(')[0]);
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

        public void UpdateItem(string itemName, int newQuantity)
        {
            if (newQuantity == 0)
            {
                RemoveItem(itemName);
            }
            else
            {
                GetItem(itemName).quantity = newQuantity;
            }
        }

        public InventoryItem GetItem(string itemName)
        {
            InventoryItem result = null;
            foreach (InventoryItem item in inventory)
            {
                if (item.name.Split('(')[0] == itemName)
                {
                    result = item;
                }
            }
            return result;
        }
    }
}