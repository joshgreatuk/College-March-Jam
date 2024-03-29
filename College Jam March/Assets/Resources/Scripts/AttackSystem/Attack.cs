using System;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSystem
{
    [CreateAssetMenu(fileName = "NewAttack", menuName = "Oasis/Attack", order = 1)]
    public class Attack : ScriptableObject
    {
        //Attack vars
        public AttackType attackType = AttackType.Melee;
        public int attackDamage = 20;
        public float attackRange = 1f;

        public int attackSplashDamage = 10;
        public float attackSplashRadius = 1f;

        public float attackCooldown = 0;
        public float attackStun = 1f;
        public float critChance = 0.1f;

        //Projectile Options
        public float projectileArch = 1f;
        public float projectileSpeed = 1f;
        public GameObject projectilePrefab;
        public bool projectileRandomRotation = false;
        public bool projectileGravity = true;
    }
    
    public enum AttackType
    {
        Melee,
        Ranged,
        //TODO
        Area,
        AreaRanged,
        CompanionAttack
    }
}