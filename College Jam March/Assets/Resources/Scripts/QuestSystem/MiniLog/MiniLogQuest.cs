using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QuestSystem
{
    public class MiniLogQuest : MonoBehaviour 
    {
        //For references on the prefab
        public TMP_Text questName;
        public TMP_Text objText;
        public Transform objectiveListTransform;
        public List<MiniLogObjective> logObjectiveList = new List<MiniLogObjective>();
    }
}