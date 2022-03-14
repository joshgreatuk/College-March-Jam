using System.Collections.Generic;
using UnityEngine;

namespace AttackSystem.AttackCollection
{
    public class A_Punch : Attack
    {
        //Attack vars
        new public AttackType attackType = AttackType.Melee;
        new public int attackCooldown = 3;
        new public int attackDamage = 5;
        new public int attackSplashDamage = 0;
        new public float attackSplashRadius = 0;
        new public float attackRange = 1f;
        new public Sprite attackSprite = null;
    }
}