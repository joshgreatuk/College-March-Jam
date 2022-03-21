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

    public static UIRefs instance;
    private void Awake()
    {
        instance = this;
    }
}