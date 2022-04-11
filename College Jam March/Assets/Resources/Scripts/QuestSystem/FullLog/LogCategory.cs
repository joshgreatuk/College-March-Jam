using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuestSystem
{
    //Quest List Item
    public class LogCategory : MonoBehaviour 
    {
        //For references on the prefab
        public TMP_Text title;
        public List<LogQuest> logQuestList = new List<LogQuest>();
    }
}