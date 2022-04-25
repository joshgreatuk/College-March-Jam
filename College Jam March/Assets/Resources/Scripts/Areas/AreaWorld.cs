using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Areas
{
    public class AreaWorld : Area 
    {
        public Transform spawnPoint;
        public AreaWorld targetArea;

        new protected void Awake() { base.Awake(); destinationName = targetArea.areaName; }
    }
}