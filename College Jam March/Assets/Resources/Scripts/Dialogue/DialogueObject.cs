using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Oasis/Dialogue", order = 1)]
    public class DialogueObject : ScriptableObject
    {
        public bool autoPlay = false;
        [SerializeField]
        public List<DialogueNode> nodeList = new List<DialogueNode>();
    }
}
