using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class NPCRefs : MonoBehaviour
    {
        public static NPCRefs instance;

        private void Awake() 
        {
            instance = this;    
        }

        public List<NPCClass> npcList = new List<NPCClass>();

        public void RegisterNPC(NPCClass npc)
        {
            
        }

        public void RemoveNPC(NPCClass npc)
        {

        }
    }
}