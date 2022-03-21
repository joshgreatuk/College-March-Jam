using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Dialogue
{
    [CustomEditor(typeof(DialogueObject))]
    public class NodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DialogueObject dialogueObject = (DialogueObject)target;
            foreach (DialogueNode node in dialogueObject.nodeList)
            {
                //Display appropriate properties
                switch (node.nodeType)
                {
                    case DialogueNodeType.Speech:
                        break;
                    case DialogueNodeType.Choice:
                        break;
                    case DialogueNodeType.Trigger:
                        break;
                }
            }
        }
    }
}