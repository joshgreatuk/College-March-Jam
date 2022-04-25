using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Areas
{
    public class Area : MonoBehaviour 
    {
        public string areaName = "New Area";
        public string destinationName = "Area";
        public bool unlocked = true;
        public string lockedText;

        protected void Awake() { if (lockedText == "") lockedText = $"{areaName} is locked"; }
        
        public Scene GetScene()
        {
            return gameObject.scene;
        }
    }
}