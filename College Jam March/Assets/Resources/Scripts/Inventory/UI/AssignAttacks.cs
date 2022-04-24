using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AttackSystem;
using Inventory;
using TMPro;

namespace Inventory
{
    public class AssignAttacks : MonoBehaviour 
    {
        public WeaponItem itemWeapon;
        private int attackIndex = 0;
        public TMP_Text attackTitle;
        public TMP_Text attackText;

        public void UpdateAttack()
        {
            Attack targetAttack = itemWeapon.attackList[attackIndex];
            attackTitle.text = $"{targetAttack.publicName} ({attackIndex+1}/{itemWeapon.attackList.Count})";
            attackText.text =
                $"Damage: {targetAttack.attackDamage*itemWeapon.damageModifier}\n" +
                $"Knockback: {targetAttack.attackKnockback*itemWeapon.knockbackModifier}\n" +
                $"Cooldown: {targetAttack.attackCooldown*itemWeapon.cooldownModifier}\n" +
                $"Stun: {targetAttack.attackStun*itemWeapon.stunModifier}\n" +
                $"Crit Chance: {Mathf.RoundToInt(targetAttack.critChance*itemWeapon.critModifier*100)}";
        }

        public void B_NextAttack()
        {
            attackIndex++;
            if (attackIndex >= itemWeapon.attackList.Count) attackIndex = 0;
            UpdateAttack();
        }

        public void B_LastAttack()
        {
            attackIndex--;
            if (attackIndex <= -1) attackIndex = itemWeapon.attackList.Count-1;
            UpdateAttack();
        }
    }
}