using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Inventory;
using TMPro;
using Player;

namespace UpgradeMenus
{
    public class DamageUpgradeMenu : MonoBehaviour 
    {
        public TMP_Dropdown weaponDropdown;
        public TMP_Text componentsLeft;
        public TMP_Text upgradeText;
        public Button upgradeButton;
        [Space(5)]
        public float upgradeAmount = 0.1f;

        private List<WeaponItem> weaponList = new List<WeaponItem>();
        private WeaponItem selectedItem = null;
        private int selectedIndex = 0;
        private InventoryManager inventoryManager;

        private void Start() 
        {
            //Populate dropdown and upgrade texts
            upgradeButton.interactable = true;
            inventoryManager = PlayerRefs.instance.playerClass.playerInventory;
            foreach (InventoryItem item in inventoryManager.inventory)
            {
                if (item.category == InventoryCategories.Weapon) weaponList.Add((WeaponItem)item);
            }
            foreach (WeaponItem item in weaponList)
            {
                weaponDropdown.options.Add(new TMP_Dropdown.OptionData(item.publicName));
            }
            UpdateMenu(0);
        }

        public void UpdateMenu(int newItem)
        {
            InventoryItem componentItem = inventoryManager.GetItem("StalkerClaw");
            if (componentItem == null) 
            {
                componentsLeft.text = $"You have 0 left";
                upgradeButton.interactable = false;
            }
            else componentsLeft.text = $"You have {componentItem.quantity} left";
            selectedItem = weaponList[newItem];
            upgradeText.text = $"{selectedItem.publicName} damage: ({selectedItem.damageModifier}) => ({(selectedItem.damageModifier + upgradeAmount).ToString()})";
            selectedIndex = newItem;
        }

        public void B_CloseMenu()
        {
            PlayerRefs.instance.playerClass.UnlockMovement();
            Destroy(this.gameObject);
        }

        public void B_Upgrade()
        {
            InventoryItem componentItem = inventoryManager.GetItem("StalkerClaw");
            if (componentItem != null)
            {
                inventoryManager.UpdateItem("StalkerClaw", componentItem.quantity - 1);
                selectedItem.damageModifier += upgradeAmount;
                UpdateMenu(selectedIndex);
            }
        }
    }
}
