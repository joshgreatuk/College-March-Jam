using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using TMPro;
using Dialogue;

namespace AI
{
    ///TODO
    ///Figure out a way to seperate NPCs into baseNPC, quest givers, shops
    ///Quests and shops can be opened with the decision dialogue
    ///OR quest givers will open a quest menu, shops will open their own menu straight away
    ///These are extension classes of NPCClass
    public class NPCClass : BaseAI
    {
        [Header("NPC AI")]
        public string npcReference = "";
        public string npcName = "No Name";
        public DialogueObject npcDialogue;
        public bool canTalk = true;
        public bool zoomToNpc = true;

        private GameObject nameBar;
        private TMP_Text nameText;
        
        private Vector3 nameBarRotBefore;
        
        private void Start() 
        {
            nameBar = Instantiate(Prefabs.instance.nameBar);
            nameBar.transform.position = UIPoint.position;
            nameBar.transform.SetParent(Prefabs.instance.worldCanvas.transform);
            nameText = nameBar.transform.Find("NameText").GetComponent<TMP_Text>();
            nameText.text = npcName;
        }

        public void NameBarToZoom(Vector3 eulers)
        {
            nameBarRotBefore = nameBar.transform.eulerAngles;
            nameBar.transform.eulerAngles = eulers;
        }

        public void NameBarReset()
        {
            nameBar.transform.eulerAngles = nameBarRotBefore;
        }
    }
}