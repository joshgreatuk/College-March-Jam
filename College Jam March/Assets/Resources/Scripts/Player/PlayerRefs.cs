using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Run in editor so that AttackCollection.cs can get the prefabs
namespace Player
{
    public class PlayerRefs : MonoBehaviour
    {
        public GameObject playerObject;
        public PlayerClass playerClass;

        public static PlayerRefs instance;
        [ExecuteAlways]
        private void Awake() 
        {
            instance = this;
        }

        
    }   
}