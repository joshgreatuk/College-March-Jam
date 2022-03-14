using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSystem;
using Effects;
using TMPro;

namespace Enemy
{
    ///ENEMY NOTES
    ///Enemies with melee attacks will follow the player and back off during a cooldown
    ///Enemies with ranged attacks will fall back to just under the attack range and if the player gets too close will swap to a melee attack

    public class EnemyClass : MonoBehaviour
    {
        public Attack mainAttack;
        public Attack secondaryAttack;
        public Vector3 UIPoint;

        public int enemyHealth = 100000;

        private void Awake() 
        {
            if (transform.Find("UIPoint"))
            {
                UIPoint = transform.Find("UIPoint").position;
            }
        }

        public void EnemyHit(int damage, float stunLevel)
        {
            enemyHealth -= damage;
            //Add stun
            GameObject damagePopup = Instantiate(Prefabs.instance.damagePopup);
            damagePopup.transform.position = UIPoint;
            damagePopup.transform.parent = Prefabs.instance.worldCanvas.transform;
            TMP_Text damageText = damagePopup.GetComponent<TMP_Text>();
            damageText.text = mainAttack.attackDamage.ToString();
            UIEffects.instance.UIPhaseOut(damageText, 1f, Vector3.up, 0, 0, 2);
        }
    }
}