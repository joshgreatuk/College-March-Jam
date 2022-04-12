using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Inventory
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Oasis/ComponentItem", order = 0)]
    public class ComponentItem : InventoryItem 
    {
        private void Reset() 
        {
            category = InventoryCategories.Component;    
        }
    }
}