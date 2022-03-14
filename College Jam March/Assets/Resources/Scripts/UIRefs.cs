using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRefs : MonoBehaviour
{
    //Prefab References
    public Canvas UICanvas;
    public Image mainCoolBar;
    public Image secCoolBar;

    public static UIRefs instance;
    private void Awake()
    {
        instance = this;
    }
}