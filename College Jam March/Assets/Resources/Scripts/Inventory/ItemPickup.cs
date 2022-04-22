using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class ItemPickup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public InventoryItem item;
        public int quantity = 1;
        [HideInInspector] public GameObject pickupUI = null;
        private PickupUI pickup;
        private int attackIndex = 0;
        private bool pointerOver = false;

        private void Start() 
        {
            item = Instantiate(item);
            item.quantity = quantity;    
        }

        public void PickupItem()
        {
            InventoryManager.instance.AddItem(item);
            Destroy(gameObject);
        }

        public void OnPointerEnter(PointerEventData data) 
        {
            pointerOver = true;
            if (pickupUI == null)
            {
                //Create pickup UI and populate it
                pickupUI = Instantiate(InventoryManager.instance.pickupUIPrefab, transform);
                pickupUI.transform.SetParent(InventoryManager.instance.worldCanvas.transform);
                pickup = pickupUI.GetComponent<PickupUI>();
                pickup.title.text = item.publicName;
                pickup.quantity.text = $"({quantity.ToString()})";
                pickup.nextAttack.onClick.AddListener(NextAttack);
                pickup.lastAttack.onClick.AddListener(LastAttack);
                UpdateStats();
                Player.PlayerRefs.instance.playerClass.itemPickup = this;
                Player.PlayerRefs.instance.playerClass.itemPromptShown = false;
            }
        }

        private void UpdateStats()
        {
            switch (item.category)
            {
                case InventoryCategories.Weapon:
                    WeaponItem itemWeapon = (WeaponItem)item;
                    pickup.attackNum.text = $"{itemWeapon.attackList[attackIndex].publicName} ({(attackIndex+1).ToString()}/{itemWeapon.attackList.Count})";
                    AttackSystem.Attack targetAttack = itemWeapon.attackList[attackIndex];
                    pickup.attackText.text = 
                        $"Damage: {targetAttack.attackDamage*itemWeapon.damageModifier}\n" +
                        $"Knockback: {targetAttack.attackKnockback*itemWeapon.knockbackModifier}\n" +
                        $"Cooldown: {targetAttack.attackCooldown*itemWeapon.cooldownModifier}\n" +
                        $"Stun: {targetAttack.attackStun*itemWeapon.stunModifier}\n" +
                        $"Crit Chance: {Mathf.RoundToInt(targetAttack.critChance*itemWeapon.critModifier*100)}";
                    break;
                case InventoryCategories.Component:
                    pickup.icon.sprite = item.itemIcon;
                    pickup.attackNum.gameObject.SetActive(false);
                    pickup.attackText.gameObject.SetActive(false);
                    break;
            }
        }

        public void NextAttack()
        {
            attackIndex++;
            WeaponItem itemWeapon = (WeaponItem)item;
            if (attackIndex >= itemWeapon.attackList.Count) attackIndex = 0;
            UpdateStats();
        }

        public void LastAttack()
        {
            attackIndex--;
            WeaponItem itemWeapon = (WeaponItem)item;
            if (attackIndex <= -1) attackIndex = itemWeapon.attackList.Count-1;
            UpdateStats();   
        }

        public void OnPointerExit(PointerEventData data)
        {
            pointerOver = false;
        }

        public void FixedUpdate()
        {
            if (pickupUI != null && !pickup.pointerOver && !pointerOver)
            {
                Destroy(pickupUI);
                Player.PlayerRefs.instance.playerClass.itemPickup = null;
                Player.PlayerRefs.instance.playerClass.playerInteraction.OnPickupTooltipDestroyed();
            }
        }
    }
}