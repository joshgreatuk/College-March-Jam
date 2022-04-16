using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace AttackSystem
{
    [CreateAssetMenu(fileName = "NewAttack", menuName = "Oasis/Attack", order = 1)]
    public class Attack : ScriptableObject
    {
        //Attack vars
        public AttackType attackType = AttackType.Melee;
        public string publicName = "";
        [Space(10)]
        public int attackDamage = 20;
        public float attackRange = 1f;
        [Space(10)]
        public int attackSplashDamage = 10;
        public float attackSplashRadius = 1f;
        [Space(10)]
        public float attackKnockback = 1f;
        [Space(10)]
        public float attackCooldown = 0;
        public float attackStun = 1f;
        public float critChance = 0.1f;
        [Space(15)]
        //Projectile Options
        [AllowNesting] [ShowIf("attackType", AttackType.Ranged)] public float projectileArch = 1f;
        [AllowNesting] [ShowIf("attackType", AttackType.Ranged)] public float projectileSpeed = 1f;
        [Space(5)]
        [AllowNesting] [ShowIf("attackType", AttackType.Ranged)] public GameObject projectilePrefab;
        [Space(5)]
        [AllowNesting] [ShowIf("attackType", AttackType.Ranged)] public bool projectileRandomRotation = false;
        [AllowNesting] [ShowIf("attackType", AttackType.Ranged)] public bool projectileGravity = true;
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