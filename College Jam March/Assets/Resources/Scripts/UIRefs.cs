using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRefs : MonoBehaviour
{
    //Prefab References
    public Canvas UICanvas;
    public Image mainCoolBar;
    public Image secCoolBar;
    public TMP_Text playerPrompt;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public GameObject dialogueTextPrefab;
    public GameObject dialogueDecisionPrefab;

    public static UIRefs instance;
    private void Awake()
    {
        instance = this;
    }
}