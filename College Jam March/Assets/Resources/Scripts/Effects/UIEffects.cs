using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using TMPro;

namespace Effects
{
    public class UIEffects : MonoBehaviour
    {
        public static UIEffects instance;
        private void Awake() 
        {
            instance = this;
        }

        public void UIPhaseOut(TMP_Text uiText, float phaseTime,Vector3 offsetDirection, float endScale=1,
                                float endOpacity=1, float offsetSpeed=1)
        {
            LeanTween.scale(uiText.gameObject, new Vector3 (endScale, endScale, 1), phaseTime);
            LeanTweenExt.LeanAlphaText(uiText, endOpacity, phaseTime);
            LeanTween.move(uiText.gameObject, uiText.gameObject.transform.position + (offsetDirection*offsetSpeed), phaseTime);
            StartCoroutine(DeleteAfterSecs(uiText.gameObject, phaseTime));
        }

        IEnumerator DeleteAfterSecs(GameObject toDelete, float deleteSecs)
        {
            yield return new WaitForSeconds(deleteSecs);
            DestroyImmediate(toDelete);
        }
    }
}