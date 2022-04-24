using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Areas
{
    public class AreaScene : Area 
    {
        public GameScene targetArea;
        public Vector3 spawnPoint;

        public Scene GetScene()
        {
            return SceneManager.GetActiveScene();
        }
    }
}