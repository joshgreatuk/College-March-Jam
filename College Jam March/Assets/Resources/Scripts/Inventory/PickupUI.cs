using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Inventory
{
    public class PickupUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TMP_Text title;
        public TMP_Text quantity;
        public TMP_Text attackNum;
        public TMP_Text attackText;
        public Image icon;
        public Button nextAttack;
        public Button lastAttack;

        public bool pointerOver = false;

        public void OnPointerEnter (PointerEventData data)
        { pointerOver = true; }
        public void OnPointerExit (PointerEventData data)
        { pointerOver = false; }
    }
}