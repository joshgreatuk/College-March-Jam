using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    public class InvFull : MonoBehaviour 
    {
        public Transform categoryParent;
        public Transform itemListParent;

        public InvInfo itemInfo;

        private List<InventoryItem> inventoryCached = new List<InventoryItem>();
        
        private List<InvItem> itemList = new List<InvItem>();
        private List<InvCategory> catList = new List<InvCategory>();
        private int selectedCategory = 0;
        private int selectedItem = 0;
        private int attackIndex = 0;

        public void UpdateMenu(List<InventoryItem> newInv)
        {
            inventoryCached.Clear();
            inventoryCached.AddRange(newInv);

            UpdateCategories();
            // UpdateItems(); DONE BY ^
            UpdateItemInfo();
        }

        public void UpdateCategories()
        {
            //Clear existing Categories
            if (catList.Count > 0)
            {
                foreach (InvCategory category in catList)
                {
                    Destroy(category.gameObject);
                }
                catList = new List<InvCategory>();
            }

            //Go through categories to create them

            List<string> categoryNames = System.Enum.GetNames (typeof(InventoryCategories)).ToList<string>();

            GameObject newObject = Instantiate(InventoryManager.instance.categoryPrefab, categoryParent);
            InvCategory newCategory = newObject.GetComponent<InvCategory>();
            catList.Add(newCategory);
            newCategory.catTitle.text = categoryNames[0];
            newCategory.catButton.onClick.AddListener(() => { B_SelectCategory(0); });
            categoryNames.RemoveAt(0);

            for (int i=0; i < categoryNames.Count; i++)
            {
                if (InventoryManager.instance.categories.HasFlag((InventoryCategories)(1<<i)))
                {
                    newObject = Instantiate(InventoryManager.instance.categoryPrefab, categoryParent);
                    newCategory = newObject.GetComponent<InvCategory>();
                    catList.Add(newCategory);
                    newCategory.catTitle.text = categoryNames[i];
                    int x = catList.Count - 1;
                    newCategory.catButton.onClick.AddListener(() => { B_SelectCategory(x); });
                }
            }

            B_SelectCategory(selectedCategory);
        }

        public void B_SelectCategory(int selectedCat)
        {
            catList[selectedCategory].catButton.interactable = true;
            selectedCategory = selectedCat;
            catList[selectedCategory].catButton.interactable = false;
            B_SelectItem(0);
            UpdateItems();
        }

        public void UpdateItems()
        {
            //Go through items to update quantities, check for new ones and ones that are no longer there
            foreach (InvItem item in itemList)
            {
                Destroy(item.gameObject);
            }
            itemList = new List<InvItem>();

            for (int i=0; i < inventoryCached.Count; i++)
            {
                if ((int)inventoryCached[i].category == selectedCategory || selectedCategory == 0)
                {
                    if (!inventoryCached[i].hidden) AddItem(inventoryCached[i]);
                }
            }
            UpdateItemInfo();
        }

        public void AddItem(InventoryItem item)
        {
            GameObject newObject = Instantiate(InventoryManager.instance.itemPrefab, itemListParent);
            InvItem newItem = newObject.GetComponent<InvItem>();
            itemList.Add(newItem);
            newItem.item = item;
            newItem.itemName.text = item.publicName;
            newItem.itemIcon.sprite = item.itemIcon;
            newItem.itemQuantity.text = item.quantity.ToString();
            int x = itemList.Count - 1;
            newItem.itemButton.onClick.AddListener(() => { B_SelectItem(x); });
            newItem.tooltipScript.passItem = item;
        }

        public void RemoveItem(int index)
        {
            Destroy(itemList[index].gameObject);
            itemList.RemoveAt(index);
            UpdateItemButtons();
        }

        public void UpdateItemButtons()
        {
            //Called when an item is removed from the list, so that there is no index issues
            for (int i=0; i < itemList.Count; i++)
            {
                itemList[i].itemButton.onClick.RemoveAllListeners();
                int x = i;
                itemList[i].itemButton.onClick.AddListener(() => { B_SelectItem(x); });
            }
        }

        public void UpdateItemInfo()
        {
            //Update item info to swap item
            if (itemList.Count < 1)
            {
                itemInfo.itemObject.SetActive(false);
                itemInfo.attackObject.SetActive(false);
                itemInfo.leftAssignButton.gameObject.SetActive(false);
                itemInfo.rightAssignButton.gameObject.SetActive(false);
                return;
            }

            itemList[selectedItem].itemButton.interactable = false;

            itemInfo.itemObject.SetActive(true);
            InventoryItem currentItem = itemList[selectedItem].item;
            itemInfo.itemTitle.text = currentItem.publicName;
            itemInfo.itemDescription.text = $"Description:\n{currentItem.itemDescription}";

            if (currentItem.category == InventoryCategories.Weapon)
            {
                itemInfo.attackObject.SetActive(true);
                itemInfo.leftAssignButton.gameObject.SetActive(true);
                itemInfo.rightAssignButton.gameObject.SetActive(true);
                UpdateAttack();
            }
            else
            {
                itemInfo.attackObject.SetActive(false);
                itemInfo.leftAssignButton.gameObject.SetActive(false);
                itemInfo.rightAssignButton.gameObject.SetActive(false);
            }
            itemInfo.lastButton.onClick.RemoveAllListeners();
            itemInfo.lastButton.onClick.AddListener(B_LastAttack);
            itemInfo.nextButton.onClick.RemoveAllListeners();
            itemInfo.nextButton.onClick.AddListener(B_NextAttack);
        }

        public void UpdateAttack()
        {
            WeaponItem itemWeapon = (WeaponItem)itemList[selectedItem].item;
            itemInfo.attackTitle.text = $"{itemWeapon.attackList[attackIndex].publicName} ({(attackIndex+1).ToString()}/{itemWeapon.attackList.Count})";
            AttackSystem.Attack targetAttack = itemWeapon.attackList[attackIndex];
            itemInfo.attackText.text = 
                $"Damage: {targetAttack.attackDamage*itemWeapon.damageModifier}\n" +
                $"Knockback: {targetAttack.attackKnockback*itemWeapon.knockbackModifier}\n" +
                $"Cooldown: {targetAttack.attackCooldown*itemWeapon.cooldownModifier}\n" +
                $"Stun: {targetAttack.attackStun*itemWeapon.stunModifier}\n" +
                $"Crit Chance: {Mathf.RoundToInt(targetAttack.critChance*itemWeapon.critModifier*100)}";
        }

        public void B_SelectItem(int selItem)
        {
            if (itemList.Count > 0 ) itemList[selectedItem].itemButton.interactable = true;
            selectedItem = selItem;
            if (itemList.Count > 0 ) itemList[selectedItem].itemButton.interactable = false;
            attackIndex = 0;
            UpdateItemInfo();
        }
        
        public void B_NextAttack()
        {   
            attackIndex++;
            WeaponItem itemWeapon = (WeaponItem)itemList[selectedItem].item;
            if (attackIndex >= itemWeapon.attackList.Count) attackIndex = 0;
            UpdateAttack();
        }

        public void B_LastAttack()
        {
            attackIndex--;
            WeaponItem itemWeapon = (WeaponItem)itemList[selectedItem].item;
            if (attackIndex <= -1) attackIndex = itemWeapon.attackList.Count-1;
            UpdateAttack();
        }

        public void B_AssignLeft()
        {
            InventoryManager.instance.assignmentMenu.AssignLeft((WeaponItem)itemList[selectedItem].item);
        }

        public void B_AssignRight()
        {
            InventoryManager.instance.assignmentMenu.AssignRight((WeaponItem)itemList[selectedItem].item);
        }
    }
}