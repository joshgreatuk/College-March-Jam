using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Player
{
    public class PlayerPanel : MonoBehaviour 
    {
        public TMP_Text playerName;
        public TMP_Text levelText;
        [Space(5)]
        public Image leftWeaponCooldown;
        public Image rightWeaponCooldown;
        [Space(5)]
        public TMP_Text xpText;
        public SlicedFilledImage xpBarValue;
        [Space(5)]
        public TMP_Text hpText;
        public SlicedFilledImage hpBarValue;
    }
}