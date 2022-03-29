using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

namespace AttackSystem
{
    [RequireComponent(typeof(Projectile))]
    public class Projectile : MonoBehaviour
    {
        public Attack projAttack;
        public GameObject projSource;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                EnemyClass enemyScript = other.gameObject.GetComponent<EnemyClass>();
                enemyScript.EnemyHit(projAttack.attackDamage, projAttack.attackStun,
                                     UnityEngine.Random.Range(0.00f, 1.00f)<=projAttack.critChance);
            }
            if (other.gameObject != projSource)
            { Destroy(gameObject); }
            
        }
    }
}