using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AttackSystem;
using Effects;
using TMPro;
using Player;

namespace AI
{
    ///ENEMY NOTES
    ///Enemies with melee attacks will follow the player and back off during a cooldown
    ///Enemies with ranged attacks will fall back to just under the attack range and if the player gets too close will swap to a melee attack

    public class EnemyClass : BaseAI
    {
        [Header("Enemy AI")]
        public AttackList mainSelect;
        [SerializeReference]
        private Attack mainAttack;
        private bool mainCooldown = false;

        public AttackList secondarySelect;
        [SerializeReference]
        private Attack secondaryAttack;
        private bool secondaryCooldown = false;

        private Image healthBarValue;
        private TMP_Text healthBarText;

        public float enemyMaxHealth = 100;
        public float enemyHealth = 100;

        [ExecuteAlways]
        private void OnValidate() 
        {
            mainAttack = AttackCollection.GetAttack(mainSelect);
            secondaryAttack = AttackCollection.GetAttack(secondarySelect);
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
            UIEffects.instance.UIPhaseOut(damageText, 1f, Vector3.up, 0, 0, 2, true);
            HealthBarUpdate();
        }

        public void HealthBarUpdate()
        {
            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
                //DIE
            }
            LeanTween.scaleX(healthBarValue.gameObject, enemyHealth/enemyMaxHealth, (1f-(enemyHealth/enemyMaxHealth))/2);   
            healthBarText.text = "HP: " + enemyHealth.ToString() + "/" + enemyMaxHealth.ToString();
        }

        IEnumerator fireProjectile(Transform projectile, Vector3 target)
        {
            //Wind up time
            yield return new WaitForSeconds(0.1f);

            //Get the attack we are using
            Attack attackToUse = projectile.GetComponent<Projectile>().projAttack;

            //Calculate the distance to the target first
            float targetDistance =  Vector3.Distance(transform.position, target);
            if (targetDistance > attackToUse.attackRange)
            {
                targetDistance = attackToUse.attackRange;
            }

            //Rotate projectile towards target
            projectile.LookAt(target);

            //Calculate angle the projectile should go at
            Vector3 projectileVector = (projectile.forward+(projectile.up/attackToUse.projectileArch));
            float projectileAngle = Vector3.Angle(projectile.forward, projectileVector);

            //Calculate velocity to throw the projectile at the desired angle 
            float projectileVelocity = targetDistance * attackToUse.projectileSpeed / 1.25f;
            
            //Get Rigidbody of projectile
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
            projectileRB.AddForce(projectileVector * projectileVelocity, ForceMode.Impulse);
            
            //Calculate flight time
            //float flightDuration = targetDistance / velocityX;
            //float elapsedTime = 0f;
            // while (elapsedTime < flightDuration)
            // {
            //     projectile.Translate(0, (velocityY - (gravity * elapsedTime)) * Time.deltaTime, velocityX * Time.deltaTime);
            //     elapsedTime += Time.deltaTime;
            //     yield return null;
            // }
        }

        IEnumerator mainAttackCooldown(float cooldown) 
        { 
            mainCooldown = true; 
            UIRefs.instance.mainCoolBar.transform.localScale = new Vector3 (1, 1, 1);
            LeanTween.scaleX(UIRefs.instance.mainCoolBar.gameObject, 0f, mainAttack.attackCooldown);
            yield return new WaitForSeconds(cooldown); 
            mainCooldown = false; 
        }

        IEnumerator secondAttackCooldown(float cooldown) 
        { 
            secondaryCooldown = true;
            UIRefs.instance.secCoolBar.transform.localScale = new Vector3 (1, 1, 1);
            LeanTween.scaleX(UIRefs.instance.secCoolBar.gameObject, 0f, secondaryAttack.attackCooldown);
            yield return new WaitForSeconds(cooldown); 
            secondaryCooldown = false; 
        }
    }
}