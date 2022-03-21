using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AttackSystem;

///USE JSONUtility

public class DialogueLister : EditorWindow
{
    [MenuItem("Dialogue/DialogueList")]
    private static void ShowWindow()
    {
        var window = GetWindow<DialogueLister>();
        window.titleContent = new GUIContent("Dialogue List");
        window.Show();
    }

    private void OnGUI() 
    {
        
    }
}

public class DialogueEditor : EditorWindow 
{
    [MenuItem("Dialogue/DialogueEditor")]
    private static void ShowWindow() 
    {
        var window = GetWindow<DialogueEditor>();
        window.titleContent = new GUIContent("Dialogue Editor");
        window.Show();
    }

    private void OnGUI() 
    {
        
    }
}