using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QuestSystem
{
    public class BoardCategory : MonoBehaviour 
    {
        public string catName;
        public TMP_Text nameText;

        public List<BoardQuest> quests = new List<BoardQuest>();
    }
}