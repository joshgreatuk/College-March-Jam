using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using TMPro;
using Dialogue;

namespace AI
{
    public class NPCClass : BaseAI
    {
        [Header("NPC AI")]
        public string npcName = "No Name";
        public DialogueObject npcDialogue;

        private TMP_Text nameText;
        
        private void Start() 
        {
            GameObject nameBar = Instantiate(Prefabs.instance.nameBar);
            nameBar.transform.position = UIPoint;
            nameBar.transform.SetParent(Prefabs.instance.worldCanvas.transform);
            nameText = nameBar.transform.Find("NameText").GetComponent<TMP_Text>();
            nameText.text = npcName;
        }
    }
}