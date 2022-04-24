using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuestSystem
{
    public class BoardQuest : MonoBehaviour 
    {
        public Image questIcon;
        public TMP_Text questName;
        public TMP_Text objCount;
        public Button questButton;

        public Quest quest;
    }
}