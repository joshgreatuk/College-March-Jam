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
        public int attackDamage = 20;
        public float attackRange = 1f;

        public int attackSplashDamage = 10;
        public float attackSplashRadius = 1f;

        public float attackCooldown = 0;
        public float attackStun = 1f;
        public float critChance = 0.1f;

        //Projectile Options
        public float projectileArch = 1f;
    }
    
    public enum AttackType
    {
        Melee,
        Ranged,
        Area
    }
}