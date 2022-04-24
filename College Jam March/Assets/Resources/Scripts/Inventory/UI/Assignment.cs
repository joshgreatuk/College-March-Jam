using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AttackSystem;
using Inventory;
using TMPro;
using Player;

namespace Inventory
{
    public class Assignment : MonoBehaviour 
    {
        public Sprite defaultIcon;
        [Space(5)]
        public AssignAttacks leftAttacks;
        public Image leftIcon;
        public TMP_Text leftTitle;
        public Button leftUnassign;
        [Space(5)]
        public AssignAttacks rightAttacks;
        public Image rightIcon;
        public TMP_Text rightTitle;
        public Button rightUnassign;
        [Space(5)]
        public PlayerAssignment playerAssignment;

        private void Start() 
        {
            AssignLeft(playerAssignment.leftHandItem);
            leftAttacks.UpdateAttack();
            AssignRight(playerAssignment.rightHandItem);
            rightAttacks.UpdateAttack();
        }

        public void AssignLeft(WeaponItem newItem)
        {
            if ((rightAttacks.itemWeapon == newItem && newItem.quantity > 1) || rightAttacks.itemWeapon != newItem)
            {
                playerAssignment.leftHandItem = newItem;
                leftAttacks.itemWeapon = newItem;
                leftAttacks.UpdateAttack();
                if (newItem.itemIcon != null) leftIcon.sprite = newItem.itemIcon;
                else leftIcon.sprite = defaultIcon;
                leftTitle.text = newItem.publicName;
            }
        }

        public void AssignRight(WeaponItem newItem)
        {
            if ((leftAttacks.itemWeapon == newItem && newItem.quantity > 1) || leftAttacks.itemWeapon != newItem)
            {
                playerAssignment.rightHandItem = newItem;
                rightAttacks.itemWeapon = newItem;
                rightAttacks.UpdateAttack();
                if (newItem.itemIcon != null) rightIcon.sprite = newItem.itemIcon;
                else rightIcon.sprite = defaultIcon;
                rightTitle.text = newItem.publicName;
            }  
        }

        public void B_UnassignLeft()
        {
            AssignLeft(playerAssignment.defaultHandItem);
        }

        public void B_UnassignRight()
        {
            AssignRight(playerAssignment.defaultHandItem);
        }
    }
}