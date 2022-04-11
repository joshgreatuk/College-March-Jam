using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QuestSystem
{
    public class MiniLogCategory : MonoBehaviour 
    {
        //For references on the prefab
        public TMP_Text catName;
        public List<MiniLogQuest> logQuestList = new List<MiniLogQuest>();
    }
}