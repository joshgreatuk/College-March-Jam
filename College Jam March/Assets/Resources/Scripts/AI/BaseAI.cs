using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using TMPro;

namespace AI
{
    public class BaseAI : MonoBehaviour
    {
        [Header("BaseAI")]
        public float viewDistance = 10f;
        public bool lookAtPlayer = true;
        public bool lookThroughObjects = true;
        public bool resetViewAfter = false;

        protected Vector3 UIPoint;
        protected GameObject playerObject;

        private Vector3 originalView;
        private bool viewReset = true;

        private void Awake() 
        {
            UIPoint = transform.Find("UIPoint").position;
            originalView = transform.position + transform.forward;
        }

        private void FixedUpdate() 
        {
            if (playerObject == null)
            { playerObject = PlayerRefs.instance.playerObject; }
            
            if (lookAtPlayer)
            {
                if (Vector3.Distance(transform.position, playerObject.transform.position) <= viewDistance)
                {
                    viewReset = false;
                    if (lookThroughObjects)
                    {
                        transform.LookAt(playerObject.transform.position);
                    }
                    else
                    {
                        RaycastHit hit;
                        Vector3 rayOrigin = transform.position + transform.forward;
                        Vector3 rayDirection = (playerObject.transform.position - rayOrigin).normalized;
                        Debug.DrawRay(rayOrigin, rayDirection, Color.red, 1);
                        if (Physics.Raycast(rayOrigin, rayDirection, out hit, viewDistance))
                        {
                            if (hit.collider.gameObject.tag == "Player")
                            {
                                transform.LookAt(playerObject.transform.position);
                            }
                            else if (resetViewAfter)
                            {
                                ResetView();
                            }
                        }
                    }
                }
                else if (!viewReset && resetViewAfter)
                {
                    ResetView();
                    viewReset = true;
                }
            }
            
        }

        private void ResetView()
        {
            transform.LookAt(originalView);
        } 
    }
}