using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text versionText;

    private void Start() 
    {
        versionText.text = $"Version: Alpha {Application.version}";
    }

    public void B_PlayAlpha() { GameManager.instance.LoadAlpha(); }
    public void B_TestWorld() { GameManager.instance.LoadTest(); }
    public void B_OptionsMenu() {} //NYI
    public void B_ExitGame() { GameManager.instance.ExitGame(); }
}