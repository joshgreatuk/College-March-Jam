using System;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSystem
{
    [Serializable]
    public class Attack
    {
        //Attack vars
        public string attackName = "Attack";
        public AttackType attackType;
        public int attackCooldown = 0;
        public int attackDamage = 20;
        public int attackSplashDamage = 10;
        public float attackSplashRadius = 1f;
        public float attackRange = 1f;
        public float attackStun = 1f;
        public Sprite attackSprite = null;
    }
    
    public enum AttackType
    {
        Melee,
        Ranged,
        Area
    }
}