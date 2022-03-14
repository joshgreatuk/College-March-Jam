using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using TMPro;

namespace Effects
{
    public class UIEffects : MonoBehaviour
    {
        public void UIPhaseOut(TMP_Text uiText, float phaseTime, float endScale, float endOpacity)
        {
            LeanTween.scale(uiText.gameObject, new Vector3 (endScale, endScale, 1), phaseTime);
            LeanTweenExt.LeanAlphaText(uiText, endOpacity, phaseTime);
            StartCoroutine(DeleteAfterSecs(uiText.gameObject, phaseTime));
        }

        IEnumerator DeleteAfterSecs(GameObject toDelete, float deleteSecs)
        {
            yield return new WaitForSeconds(deleteSecs);
            DestroyImmediate(toDelete);
        }
    }
}