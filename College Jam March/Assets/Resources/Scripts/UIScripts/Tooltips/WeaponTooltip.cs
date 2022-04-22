using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Inventory;

public class WeaponTooltip : Tooltip
{
    public WeaponItem referenceWeapon;
    public TMP_Text attackNum;
    public Button lastButton;
    public Button nextButton;

    private int attackIndex = 0;

    public override void UpdateTooltip()
    {
        //Read from player class active weapons
        title.text = referenceWeapon.publicName;
        attackNum.text = $"{referenceWeapon.attackList[attackIndex].publicName} ({(attackIndex+1).ToString()}/{referenceWeapon.attackList.Count})";
        AttackSystem.Attack targetAttack = referenceWeapon.attackList[attackIndex];
            text.text = 
                $"Damage: {targetAttack.attackDamage*referenceWeapon.damageModifier}\n" +
                $"Knockback: {targetAttack.attackKnockback*referenceWeapon.knockbackModifier}\n" +
                $"Cooldown: {targetAttack.attackCooldown*referenceWeapon.cooldownModifier}\n" +
                $"Stun: {targetAttack.attackStun*referenceWeapon.stunModifier}\n" +
                $"Crit Chance: {Mathf.RoundToInt(targetAttack.critChance*referenceWeapon.critModifier*100)}";
    }

    public void LastAttack()
    {
        attackIndex--;
        if (attackIndex <= -1) attackIndex = referenceWeapon.attackList.Count-1;
        UpdateTooltip();
    }

    public void NextAttack()
    {
        attackIndex++;
        if (attackIndex >= referenceWeapon.attackList.Count) attackIndex = 0;
        UpdateTooltip();
    }
}