using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSystem;
using Inventory;

namespace AI
{
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "Oasis/Enemy", order = 1)]
    public class EnemyType : ScriptableObject
    {
        public Attack mainAttack;
        public Attack secondaryAttack;
        public float enemyMaxHealth = 100;
        public float enemyHealth = 100;

        public List<EnemyDrop> dropItems = new List<EnemyDrop>();
    }

    [Serializable]
    public class EnemyDrop
    {
        public InventoryItem item;
        [Range(0,1)] public float dropChance = 0.25f;
        public int minimumDropped = 1;
        public int maximumDropped = 1;
    }
}