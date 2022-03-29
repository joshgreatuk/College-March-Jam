using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EZCameraShake;
using Player;

namespace Player
{
    public class CameraFollowPlayer : MonoBehaviour
    {
        public float speed = 1;
        public GameObject toFollow;

        public bool mouseExtend = true;
        public float extendValue = 1f;
        public Vector3 followOffset;

        private float yPosition;
        private Vector3 positionOffset;
        private CameraShaker cameraShaker;
        private PlayerClass playerClass;

        private void Awake() 
        {
            cameraShaker = GetComponent<CameraShaker>();
            yPosition = transform.position.y;
            playerClass = toFollow.GetComponent<PlayerClass>();
        }

        private void Update()
        {
            //Follow toFollow Object
            Vector3 followPos = toFollow.transform.position + followOffset;
            if (transform.position != followPos && playerClass.canMove)
            {
                float interpolation = speed * Time.deltaTime;
                Vector3 position = transform.position;
                position.x = Mathf.Lerp(transform.position.x, followPos.x, interpolation);
                position.z = Mathf.Lerp(transform.position.z, followPos.z, interpolation);
                position.y = yPosition;
                position += positionOffset;
                transform.position = position;
                cameraShaker.RestPositionOffset = position;
                cameraShaker.RestRotationOffset = transform.eulerAngles;
            }
            else if (!playerClass.canMove)
            {
                cameraShaker.RestPositionOffset = transform.position;
                cameraShaker.RestRotationOffset = transform.eulerAngles;
            }
        }

        public void DoLookCalc(Vector2 mousePos)
        {
            //Move the camera towards the mouse, but to the extent of extendValue
            Vector2 screenRes = new Vector2(Screen.width, Screen.height);
            Vector2 centreRes = screenRes / 2;
            Vector2 mouseOffset = mousePos - centreRes;
            Vector2 mouseOffsetClamped = mouseOffset / centreRes;
            positionOffset = new Vector3(mouseOffsetClamped.x * extendValue, 0, mouseOffsetClamped.y * extendValue);
        }

        public void Look(InputAction.CallbackContext context)
        {
            if (toFollow.GetComponent<PlayerClass>())
            {
                if (toFollow.GetComponent<PlayerClass>().canMove)
                {
                    DoLookCalc(context.ReadValue<Vector2>());
                }
                else
                {
                    positionOffset = Vector3.zero;
                }
            }
            else
            {
                DoLookCalc(context.ReadValue<Vector2>());
            }
        }
    }
}