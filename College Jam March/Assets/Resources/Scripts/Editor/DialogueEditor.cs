#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

///USE JSONUtility

namespace Dialogue
{
    public class DialogueEditor : EditorWindow
    {
        [MenuItem("Oasis/DialogueEditor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        Vector2 scrollPosLeft;
        Vector2 scrollPosRight;

        private void OnGUI() 
        {
            GUIStyle centredText = new GUIStyle("label");
            centredText.alignment = TextAnchor.UpperCenter;

            GUILayout.Box("Dialogue Objects", centredText);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.BeginHorizontal("Dialogue Objects");
            {
                //Left panel of the editor window, for choosing dialogues
                scrollPosLeft = GUILayout.BeginScrollView(scrollPosLeft, GUI.skin.box);
                {
                    GUILayout.Label("Objects", centredText);
                    //Show the name of the dialogue and count of elements
                    //NEED DESIGN
                }
                GUILayout.EndScrollView();

                //Midle divider
                EditorGUILayout.LabelField("", GUI.skin.verticalSlider, GUILayout.Width(10));

                //Right panel of the editor window, for previewing a selected dialogue
                scrollPosRight = GUILayout.BeginScrollView(scrollPosRight, GUI.skin.box);
                {
                    GUILayout.Label("Preview", centredText);
                    //NEED DESIGN
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif