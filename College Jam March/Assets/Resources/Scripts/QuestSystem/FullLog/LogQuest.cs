using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuestSystem
{
    //Quest List Item
    public class LogQuest : MonoBehaviour 
    {
        //For references on the prefab
        public TMP_Text questName;
        public TMP_Text objectiveText;
        public TMP_Text questGiver;
        public Button questButton;
    }
}