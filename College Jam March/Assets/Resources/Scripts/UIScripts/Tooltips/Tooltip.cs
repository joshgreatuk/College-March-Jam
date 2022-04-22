using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text title;
    public TMP_Text text;

    [HideInInspector] public bool pointerOver = false;

    public virtual void UpdateTooltip()
    {
        text.text = "This is a tooltip";
    }

    public void OnPointerEnter (PointerEventData data)
        { pointerOver = true; }
        public void OnPointerExit (PointerEventData data)
        { pointerOver = false; }
}