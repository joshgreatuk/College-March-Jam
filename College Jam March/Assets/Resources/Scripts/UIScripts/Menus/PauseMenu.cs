using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public void B_Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void B_OptionsButton()
    {
        //NYI
    }

    public void B_BackToMenu()
    {
        Time.timeScale = 1;
        GameManager.instance.LoadMainMenu(Areas.AreaController.instance.areaList[0].GetScene());
    }
}