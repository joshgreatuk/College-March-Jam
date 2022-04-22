using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour 
{
    public GameObject toggleObject;
    public Sprite onSprite;
    public Sprite offSprite;
    public Image buttonImage;

    public void ButtonPressed()
    {
        if (toggleObject.activeInHierarchy)
        {
            buttonImage.sprite = offSprite;
            toggleObject.SetActive(false);
        }
        else
        {
            buttonImage.sprite = onSprite;
            toggleObject.SetActive(true);
        }
    }
}