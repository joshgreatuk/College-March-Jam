using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Areas
{
    public class Area : MonoBehaviour 
    {
        public string areaName = "New Area";
        public string destinationName = "Area";
        public bool unlocked = true;
        public string lockedText;

        protected void Awake() { lockedText = $"{areaName} is locked"; }
    }
}