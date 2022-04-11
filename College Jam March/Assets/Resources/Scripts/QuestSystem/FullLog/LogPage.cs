using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QuestSystem
{
    public class LogPage : MonoBehaviour 
    {
        //For references on the prefab
        public Transform questList;
        public Transform questInfo;
        public TMP_Text infoTitle;
        public List<LogCategory> logListObjects = new List<LogCategory>();
        public List<GameObject> logInfoObjects = new List<GameObject>();
    }
}