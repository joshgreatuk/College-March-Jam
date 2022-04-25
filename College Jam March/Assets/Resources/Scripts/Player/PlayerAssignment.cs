using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AttackSystem;
using Inventory;

namespace Player
{
    public class PlayerAssignment : MonoBehaviour 
    {
        public WeaponItem defaultHandItem;
        [Space(10)]
        public WeaponItem leftHandItem;
        public WeaponItem rightHandItem;

        private void Awake() 
        {
            if (leftHandItem == null) leftHandItem = defaultHandItem;
            if (rightHandItem == null) rightHandItem = defaultHandItem;    
        }
    }
}