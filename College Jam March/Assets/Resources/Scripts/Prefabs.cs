using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour
{
    //Prefab References
    public Canvas worldCanvas;
    public GameObject damagePopup;
    public GameObject healthBar;

    public static Prefabs instance;
    private void Awake()
    {
        instance = this;
    }
}