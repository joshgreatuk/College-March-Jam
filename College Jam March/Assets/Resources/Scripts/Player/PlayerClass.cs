using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using TMPro;
using AttackSystem;
using AI;
using QuestSystem;
using NaughtyAttributes;
using Inventory;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerClass : MonoBehaviour
    {
        public Logger logger;

        //Player variables
        [Header("Player Stats")]
        public int playerMaxHealth = 100;
        public int playerHealth = 100;
        public int playerLevel = 1;
        public float playerXp = 0;
        public float nextLevelXp = 300;

        public float playerMultiplier = 1;
        public float playerCritMultiplier = 1.5f;
        public float playerSpeed = 5;

        [ReadOnly] public bool canMove = true;

        [Header("Player Attacks")]
        private bool mainCooldown = false;

        private bool secondaryCooldown = false;

        [Header("Player References")]
        public Camera playerCamera;
        public TMP_Text playerPrompt;
        public Image playerPromptBack;
        public InventoryManager playerInventory;
        public PlayerPanel playerPanel;
        public PlayerAssignment playerAssignment;
        public PlayerInteraction playerInteraction;
        public GameObject pauseMenu;

        private Rigidbody playerRb;
        private Camera cameraComp;
        private float zDepth;
        private Vector2 moveVector;
        private Vector3 playerMouseOffset;
        private Transform throwPoint;

        public float cameraZoomTime = 1f;

        public bool debugMode = false;

        [ReadOnly] public ItemPickup itemPickup = null;
        [ReadOnly] public bool itemPromptShown = false;

        [ReadOnly] public Transform cameraZoomPoint;
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
            FreezePlayer();
            UpdatePlayerPanel();
        }

        private void Start() 
        {
            if (debugMode) { SpawnPlayer(); }
        }

        private void Update() 
        {
            if (canMove)
            {
                Vector3 newVelocity = new Vector3(moveVector.x, 0, moveVector.y) * playerSpeed;
                newVelocity.y = playerRb.velocity.y;
                playerRb.velocity = newVelocity;
            }   
        }

        private void FixedUpdate() 
        {
            if (itemPickup != null)
            {
                if (itemPromptShown && Vector3.Distance(transform.position, itemPickup.transform.position) > 3f)
                {
                    playerInteraction.OnPickupTooltipDestroyed();
                    itemPromptShown = false;
                }
                else if (!itemPromptShown && Vector3.Distance(transform.position, itemPickup.transform.position) <= 3f)
                {
                    playerInteraction.OnPickupTooltip(itemPickup.item);
                    itemPromptShown = true;
                }
            }    
        }

        public void PauseButton()
        {
            if (pauseMenu.activeSelf)
            {
                UnlockMovement();
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                LockMovement();
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }

        public void FreezePlayer()
        {
            playerRb.constraints = RigidbodyConstraints.FreezeAll;
        }

        public Vector3 spawnPointReference;

        public void SpawnPlayer()
        {
            Vector3 position = Areas.AreaController.instance.spawnPoints[0].position;
            foreach (Transform spawn in Areas.AreaController.instance.spawnPoints)
            {
                if (Vector3.Distance(spawnPointReference, spawn.position) < Vector3.Distance(spawnPointReference, position))
                {
                    position = spawn.position;
                }
            }
            transform.position = position;
            position.y = playerCamera.transform.position.y;
            playerCamera.transform.position = position;
            UnfreezePlayer();
        }

        public void UnfreezePlayer()
        {
            playerRb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void UnlockMovement()
        {
            canMove = true;
            PlayerMessages.instance.canPlay = true;
            QuestLog.instance.questLog.SetActive(true);
            playerPrompt.gameObject.SetActive(true);
            playerPromptBack.gameObject.SetActive(true);
        }

        public void LockMovement(bool removeUI=false)
        {
            canMove = false;
            PlayerMessages.instance.canPlay = false;
            if (removeUI)
            {
                QuestLog.instance.questLog.SetActive(false);
                playerPrompt.gameObject.SetActive(false);
                playerPromptBack.gameObject.SetActive(false);
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

            //Add check for level up here
            if (playerXp >= nextLevelXp)
            {
                float xpOverlap = playerXp - nextLevelXp;
                nextLevelXp *= 1.5f;
                playerLevel += 1;
                playerXp = xpOverlap;
                GameObject damagePopup = Instantiate(Prefabs.instance.uiPopup, playerPanel.transform);
                damagePopup.transform.position = playerPanel.levelText.transform.position;
                damagePopup.transform.Translate(Vector3.up*50, Space.Self);
                TMP_Text damageText = damagePopup.GetComponent<TMP_Text>();
                damageText.text = "Level Up!";
                UIEffects.instance.UIPhaseOut(damageText, 5f, Vector3.up*50, 0, 0, 2, true);
                AddXP(0);
            }
            UpdatePlayerPanel();
        }

        public void PlayerHit(int damage)
        {
            UpdatePlayerPanel();
        }

        public void UpdatePlayerPanel()
        {
            playerPanel.levelText.text = $"Level {playerLevel}";

            playerPanel.xpText.text = $"XP ({playerXp}/{nextLevelXp})";
            playerPanel.xpBarValue.fillAmount = playerXp/nextLevelXp;

            playerPanel.hpText.text = $"HP ({playerHealth}/{playerMaxHealth})";
            playerPanel.hpBarValue.fillAmount = (float)playerHealth/(float)playerMaxHealth;
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
                WeaponItem item = (WeaponItem)playerAssignment.leftHandItem;
                if (context.interaction is HoldInteraction && item.attackList.Count >= 2)
                {
                    Attack attack = item.attackList[1];
                    FireAttack(attack);
                    StartCoroutine(mainAttackCooldown(attack.attackCooldown));
                }
                else if (context.interaction is TapInteraction && item.attackList.Count >= 1)
                {
                    Attack attack = item.attackList[0];
                    FireAttack(attack);
                    StartCoroutine(mainAttackCooldown(attack.attackCooldown));
                }
                else if (context.interaction is MultiTapInteraction && item.attackList.Count >= 3)
                {
                    Attack attack = item.attackList[2];
                    FireAttack(attack);
                    StartCoroutine(mainAttackCooldown(attack.attackCooldown));
                }
            }
        }

        public void SecondAttack(InputAction.CallbackContext context)
        {
            if (!secondaryCooldown && context.performed && canMove)
            {
                WeaponItem item = (WeaponItem)playerAssignment.rightHandItem;
                if (context.interaction is HoldInteraction && item.attackList.Count >= 2)
                {
                    Attack attack = item.attackList[1];
                    FireAttack(attack);
                    StartCoroutine(secondAttackCooldown(attack.attackCooldown));
                }
                else if (context.interaction is TapInteraction && item.attackList.Count >= 1)
                {
                    Attack attack = item.attackList[0];
                    FireAttack(attack);
                    StartCoroutine(secondAttackCooldown(attack.attackCooldown));
                }
                else if (context.interaction is MultiTapInteraction && item.attackList.Count >= 3)
                {
                    Attack attack = item.attackList[2];
                    FireAttack(attack);
                    StartCoroutine(secondAttackCooldown(attack.attackCooldown));
                }
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
                        target.y = transform.position.y;
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
            //UIRefs.instance.mainCoolBar.transform.localScale = new Vector3 (1, 1, 1);
            //LeanTween.scaleX(UIRefs.instance.mainCoolBar.gameObject, 0f, mainAttack.attackCooldown);
            UIRefs.instance.mainCoolBar.fillAmount = 1;
            LeanTween.value(UIRefs.instance.mainCoolBar.gameObject, 1, 0, cooldown).setOnUpdate
                ( (float val)=>{ UIRefs.instance.mainCoolBar.fillAmount = val; } );
            yield return new WaitForSeconds(cooldown); 
            mainCooldown = false; 
        }

        IEnumerator secondAttackCooldown(float cooldown) 
        { 
            secondaryCooldown = true;
            //UIRefs.instance.secCoolBar.transform.localScale = new Vector3 (1, 1, 1);
            //LeanTween.scaleX(UIRefs.instance.secCoolBar.gameObject, 0f, secondaryAttack.attackCooldown);
            UIRefs.instance.secCoolBar.fillAmount = 1;
            LeanTween.value(UIRefs.instance.secCoolBar.gameObject, 1, 0, cooldown).setOnUpdate
                ( (float val)=>{ UIRefs.instance.secCoolBar.fillAmount = val; } );
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