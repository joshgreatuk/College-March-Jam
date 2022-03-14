using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackSystem;
using Enemy;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerClass : MonoBehaviour
    {
        //Player variables
        public int playerMaxHealth = 100;
        public int playerHealth;
        public int playerLevel = 1;
        public int playerXp = 0;

        public float playerMultiplier = 1;
        public float playerCritMultiplier = 1.5f;
        public float playerSpeed = 5;

        public AttackList mainSelect;
        [SerializeReference]
        private Attack mainAttack;
        private bool mainCooldown = false;

        public AttackList secondarySelect;
        [SerializeReference]
        private Attack secondaryAttack;
        private bool secondaryCooldown = false;

        public Camera playerCamera;

        private Rigidbody playerRb;
        private Camera cameraComp;
        private float zDepth;
        private Vector2 moveVector;
        private Vector3 playerMouseOffset;

        private void OnValidate() 
        {
            mainAttack = AttackCollection.GetAttack(mainSelect);
            secondaryAttack = AttackCollection.GetAttack(secondarySelect);
        }

        private void Awake() 
        {
            playerRb = GetComponent<Rigidbody>();   
            cameraComp = playerCamera.GetComponent<Camera>();
            zDepth = cameraComp.WorldToScreenPoint(playerRb.position).z;
        }

        private void Update() 
        {
            playerRb.velocity = new Vector3(moveVector.x, 0, moveVector.y) * playerSpeed;
        }
        
        public void Look(InputAction.CallbackContext context)
        {
            if (context.performed)
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
                Vector2 mousePos = Mouse.current.position.ReadValue();
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
            if (!mainCooldown && context.performed)
            {
                FireAttack(mainAttack);
                StartCoroutine(mainAttackCooldown(mainAttack.attackCooldown));
            }
        }

        public void SecondAttack(InputAction.CallbackContext context)
        {
            if (!secondaryCooldown && context.performed)
            {
                FireAttack(secondaryAttack);
                StartCoroutine(secondAttackCooldown(secondaryAttack.attackCooldown));
            }
        }

        public void FireAttack(Attack attack)
        {
            switch (mainAttack.attackType)
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
                    break;
            }
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

        public void Interact(InputAction.CallbackContext context)
        {
            
        }
    }
}