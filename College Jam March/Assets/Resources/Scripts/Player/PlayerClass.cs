using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AttackSystem;
using AttackSystem.AttackCollection;

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
        public float playerSpeed = 5;

        public Attack mainAttack = new A_Punch();
        public Attack secondaryAttack = new A_ThrowRock();

        public Camera playerCamera;

        private Rigidbody playerRb;
        private Camera cameraComp;
        private float zDepth;
        private Vector2 moveVector;
        private Vector3 playerMouseOffset;

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
            if (mainAttack != null)
            {
                switch (mainAttack.attackType)
                {
                    case AttackType.Melee:
                        
                        break;
                    case AttackType.Ranged:
                        break;
                }
            }
        }

        public void SecondAttack(InputAction.CallbackContext context)
        {

        }

        public void Interact(InputAction.CallbackContext context)
        {

        }
    }
}