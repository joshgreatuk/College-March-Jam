using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory
{
    public class InvInfo : MonoBehaviour 
    {
        public GameObject itemObject;

        public TMP_Text itemTitle;
        public TMP_Text itemDescription;

        public GameObject attackObject;

        public TMP_Text attackTitle;
        public TMP_Text attackText;

        public Button lastButton;
        public Button nextButton;
    }
}