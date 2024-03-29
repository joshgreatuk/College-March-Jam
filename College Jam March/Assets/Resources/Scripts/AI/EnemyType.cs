using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSystem;

namespace AI
{
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Oasis/Enemy", order = 1)]
    public class EnemyType : ScriptableObject
    {
        public Attack mainAttack;
        public Attack secondaryAttack;
        public float enemyMaxHealth = 100;
        public float enemyHealth = 100;
    }
}