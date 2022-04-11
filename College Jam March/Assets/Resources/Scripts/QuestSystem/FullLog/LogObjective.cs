using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuestSystem
{
    //Quest Info Item
    public class LogObjective : MonoBehaviour 
    {
        //For references on the prefab
        public TMP_Text objName;
        public TMP_Text objDescription;
        public TMP_Text objProgress;
        public Toggle objToggle;
    }
}