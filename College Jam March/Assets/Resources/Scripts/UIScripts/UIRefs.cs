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
    public Canvas WorldCanvas;
    public Image mainCoolBar;
    public Image secCoolBar;
    public TMP_Text playerPrompt;
    public Transform itemPickupTransform;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public GameObject dialogueTextPrefab;
    public GameObject dialogueDecisionPrefab;

    [Header("Scene Stuff")]
    public GameObject loadingScreen;

    public static UIRefs instance;
    private void Awake()
    {
        instance = this;
    }
}