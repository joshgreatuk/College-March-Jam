using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        private Vector3 UIPoint;
        private Image healthBarValue;
        private TMP_Text healthBarText;

        public float enemyMaxHealth = 100;
        public float enemyHealth = 100;

        private void Awake() 
        {
            if (transform.Find("UIPoint"))
            {
                UIPoint = transform.Find("UIPoint").position;
            }
        }

        private void Start() 
        {
            GameObject healthBar = Instantiate(Prefabs.instance.healthBar);
            healthBar.transform.position = UIPoint;
            healthBar.transform.SetParent(Prefabs.instance.worldCanvas.transform);
            healthBarValue = healthBar.transform.Find("HealthValue").GetComponent<Image>();
            healthBarText = healthBar.transform.Find("HealthText").GetComponent<TMP_Text>();
        }

        public void EnemyHit(int damage, float stunLevel, bool crit)
        {
            enemyHealth -= damage;
            //Add stun
            GameObject damagePopup = Instantiate(Prefabs.instance.damagePopup);
            damagePopup.transform.position = UIPoint;
            damagePopup.transform.Translate(Vector3.up, Space.Self);
            damagePopup.transform.SetParent(Prefabs.instance.worldCanvas.transform);
            TMP_Text damageText = damagePopup.GetComponent<TMP_Text>();
            if (crit)
            {
                damageText.text = "Crit!\n" + damage.ToString();
            }
            else
            {
                damageText.text = damage.ToString();
            }
            UIEffects.instance.UIPhaseOut(damageText, 1f, Vector3.up, 0, 0, 2);
            HealthBarUpdate();
        }

        public void HealthBarUpdate()
        {
            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
                //DIE
            }
            LeanTween.scaleX(healthBarValue.gameObject, enemyHealth/enemyMaxHealth, 1f-(enemyHealth/enemyMaxHealth));   
            healthBarText.text = "HP: " + enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
        }
    }
}