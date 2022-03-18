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

        protected Vector3 UIPoint;
        protected GameObject playerObject;

        private void Awake() 
        {
            if (transform.Find("UIPoint"))
            {
                UIPoint = transform.Find("UIPoint").position;
            }
        }

        private void FixedUpdate() 
        {
            if (playerObject == null)
            { playerObject = PlayerRefs.instance.playerObject; }
            
            if (Vector3.Distance(transform.position, playerObject.transform.position) <= viewDistance)
            {
                transform.LookAt(playerObject.transform.position);
            }
        }
    }
}