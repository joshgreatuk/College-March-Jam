using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Areas
{
    public class AreaController : MonoBehaviour 
    {
        public List<AreaWorld> worldAreaList = new List<AreaWorld>();

        public static AreaController instance;
        public void Awake() { instance = this; UpdateAreaList(); }

        public void UpdateAreaList()
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Area"))
            {
                if (!worldAreaList.Contains(obj.GetComponent<AreaWorld>()))
                {
                    worldAreaList.Add(obj.GetComponent<AreaWorld>());
                }
            }
        }

        public void UnlockArea(string area)
        {
            Area target = GetArea(area);
            if (target != null) target.unlocked = true;
        }

        public void LockArea(string area)
        {
            Area target = GetArea(area);
            if (target != null) target.unlocked = false;
        }

        public Area GetArea(string area)
        {
            Area result = null;
            foreach (Area current in worldAreaList)
            {
                if (current.areaName == area)
                {
                    result = current;
                }
            }
            return result;
        }
    }
}