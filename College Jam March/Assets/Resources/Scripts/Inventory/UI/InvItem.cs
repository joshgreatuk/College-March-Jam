using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory
{
    public class InvItem : MonoBehaviour 
    {
        public Button itemButton;
        public Image itemIcon;
        public TMP_Text itemName;
        public TMP_Text itemQuantity;

        public InventoryItem item;
    }
}