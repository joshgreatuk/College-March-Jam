using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Inventory
{
    //[CreateAssetMenu(fileName = "InventoryItem", menuName = "Oasis/InventoryItem", order = 0)]
    public class InventoryItem : ScriptableObject 
    {
        public string publicName = "";
        [TextArea(1,5)] public string itemDescription = "";
        public Sprite itemIcon = null;
        public int quantity = 1;
        public bool hidden = false;
        [ReadOnly] public InventoryCategories category = 0;
    }
}