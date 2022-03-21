using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Oasis/DialogueObject", order = 1)]
    public class DialogueObject : ScriptableObject
    {
        [SerializeField]
        public List<DialogueNode> nodeList = new List<DialogueNode>();
    }
}
