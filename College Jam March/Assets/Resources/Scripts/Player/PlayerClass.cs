using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackSystem;
using AI;
using NaughtyAttributes;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerClass : MonoBehaviour
    {
        public Logger logger;

        //Player variables
        public int playerMaxHealth = 100;
        [ProgressBar("Health", 100, EColor.Red)]
        public int playerHealth;
        public int playerLevel = 1;
        public float playerXp = 0;

        public float playerMultiplier = 1;
        public float playerCritMultiplier = 1.5f;
        public float playerSpeed = 5;

        public bool canMove = true;

        public Attack mainAttack;
        private bool mainCooldown = false;

        public Attack secondaryAttack;
        private bool secondaryCooldown = false;

        public Camera playerCamera;

        private Rigidbody playerRb;
        private Camera cameraComp;
        private float zDepth;
        private Vector2 moveVector;
        private Vector3 playerMouseOffset;
        private Transform throwPoint;

        public float cameraZoomTime = 1f;

        [SerializeReference]
        public Transform cameraZoomPoint;
        private Vector3 preCameraZoomPoint;
        private Vector3 preCameraZoomRotation;

        private void Awake() 
        {
            playerRb = GetComponent<Rigidbody>();   
            cameraComp = playerCamera.GetComponent<Camera>();
            zDepth = cameraComp.WorldToScreenPoint(playerRb.position).z;
            if (transform.Find("ThrowPoint"))
            {
                throwPoint = transform.Find("ThrowPoint");
            }
            if (transform.Find("CameraZoomPoint"))
            {
                cameraZoomPoint = transform.Find("CameraZoomPoint");
            }
        }

        private void Update() 
        {
            if (canMove)
            {
                playerRb.velocity = new Vector3(moveVector.x, 0, moveVector.y) * playerSpeed;
            }   
        }

        public void CameraZoomToNPC()
        {
            //Lerp down to NPCCameraPoint
            preCameraZoomPoint = cameraComp.transform.position;
            preCameraZoomRotation = cameraComp.transform.eulerAngles;
            LeanTween.move(cameraComp.gameObject, cameraZoomPoint.position, cameraZoomTime);
            LeanTween.rotate(cameraComp.gameObject, cameraZoomPoint.eulerAngles, cameraZoomTime);
        }

        public void CameraZoomToNormal()
        {
            //Lerp up to where camera was before
            LeanTween.move(cameraComp.gameObject, preCameraZoomPoint, cameraZoomTime);
            LeanTween.rotate(cameraComp.gameObject, preCameraZoomRotation, cameraZoomTime);
        }

        public void AddXP(float xpAmount)
        {
            //Add xp and check for level up
            logger.Log($"Added {xpAmount.ToString()} xp to player");
            playerXp += xpAmount;
        }
        
        public void Look(InputAction.CallbackContext context)
        {
            if (context.performed && canMove)
            {
                // RaycastHit hit;
                // Vector3 rayPoint = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, zDepth);
                // Ray ray = cameraComp.ScreenPointToRay(rayPoint);
                // if (Physics.Raycast(ray, out hit))
                // {
                //     //Add delay, its lagging the game
                //     Vector3 lookPoint = new Vector3(hit.point.x, playerRb.position.y, hit.point.z);
                //     playerRb.gameObject.transform.LookAt(lookPoint);
                // }
                Vector2 mousePos = context.ReadValue<Vector2>();
                Vector2 screenRes = new Vector2(Screen.width, Screen.height);
                Vector2 centreRes = screenRes / 2;
                Vector2 mouseOffset = mousePos - centreRes;
                Vector2 mouseOffsetClamped = mouseOffset / centreRes;
                playerMouseOffset = new Vector3(mouseOffsetClamped.x, 0, mouseOffsetClamped.y);
                transform.LookAt(transform.position + playerMouseOffset);
            }
        }

        public void MoveAction(InputAction.CallbackContext context)
        {
            moveVector = context.ReadValue<Vector2>();
        }

        public void MainAttack(InputAction.CallbackContext context)
        {
            if (!mainCooldown && context.performed && canMove)
            {
                FireAttack(mainAttack);
                StartCoroutine(mainAttackCooldown(mainAttack.attackCooldown));
            }
        }

        public void SecondAttack(InputAction.CallbackContext context)
        {
            if (!secondaryCooldown && context.performed && canMove)
            {
                FireAttack(secondaryAttack);
                StartCoroutine(secondAttackCooldown(secondaryAttack.attackCooldown));
            }
        }

        public void FireAttack(Attack attack)
        {
            switch (attack.attackType)
            {
                case AttackType.Melee:
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, attack.attackRange))
                    {
                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                            EnemyClass enemyClass = hit.collider.gameObject.GetComponent<EnemyClass>();
                            if (UnityEngine.Random.Range(0.00f, 1.00f) <= attack.critChance)
                            {
                                enemyClass.EnemyHit(Mathf.RoundToInt(attack.attackDamage*playerCritMultiplier), attack.attackStun, true);
                            }
                            else
                            {
                                enemyClass.EnemyHit(attack.attackDamage, attack.attackStun, false);
                            }
                        }
                    }
                    break;
                case AttackType.Ranged:
                    RaycastHit mouseHit;
                    if (Physics.Raycast(cameraComp.ScreenPointToRay(Mouse.current.position.ReadValue()), out mouseHit))
                    {
                        GameObject newProjectile = Instantiate(attack.projectilePrefab);
                        newProjectile.transform.position = throwPoint.position;
                        Projectile newProjClass = newProjectile.AddComponent<Projectile>();
                        newProjClass.projAttack = attack;
                        newProjClass.projSource = gameObject;
                        Vector3 target = mouseHit.point;
                        target.y += 0.5f;
                        StartCoroutine(fireProjectile(newProjectile.transform, target)); 
                    }
                    break;
            }
        }

        IEnumerator fireProjectile(Transform projectile, Vector3 target)
        {   
            //Wind up time
            yield return new WaitForSeconds(0.1f);

            //Get the attack we are using
            Projectile projectileClass = projectile.GetComponent<Projectile>();
            Attack attackToUse = projectileClass.projAttack;

            //Calculate the distance to the target first
            float targetDistance =  Vector3.Distance(transform.position, target);
            if (targetDistance > attackToUse.attackRange)
            {
                targetDistance = attackToUse.attackRange;
            }

            //Rotate projectile towards target
            projectile.LookAt(target);

            //Calculate angle the projectile should go at
            Vector3 projectileVector = (projectile.forward+(projectile.up*attackToUse.projectileArch));
            float projectileAngle = Vector3.Angle(projectile.forward, projectileVector);

            //Calculate velocity to throw the projectile at the desired angle 
            float projectileVelocity = targetDistance * attackToUse.projectileSpeed / 1.25f;
            
            //Get Rigidbody of projectile
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
            projectileVelocity *= projectileRB.mass;
            if (!attackToUse.projectileGravity)
            { projectileRB.useGravity = false; }
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

        IEnumerator stopMovementSecs(float stopTime)
        {
            canMove = false;
            yield return new WaitForSeconds(stopTime);
            canMove = true;
        }
    }
}