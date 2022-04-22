using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using UnityEngine.UI;
using TMPro;

public class UIEffects : MonoBehaviour
{
    public static UIEffects instance;
    private void Awake() 
    {
        instance = this;
    }

    public void UIPhaseOut(Image uiImage, float phaseTime,Vector3 offsetDirection, float endScale=1,
                            float endOpacity=0, float offsetSpeed=1, bool deleteObject=false)
    {
        LeanTween.scale(uiImage.gameObject, new Vector3 (endScale, endScale, 1), phaseTime);
        LeanTween.alpha(uiImage.rectTransform, endOpacity, phaseTime);
        LeanTween.move(uiImage.gameObject, uiImage.gameObject.transform.position + (offsetDirection*offsetSpeed), phaseTime);
        if (deleteObject)
        { StartCoroutine(DeleteAfterSecs(uiImage.gameObject, phaseTime)); }
    }

    public void UIPhaseOut(TMP_Text uiText, float phaseTime,Vector3 offsetDirection, float endScale=1,
                            float endOpacity=0, float offsetSpeed=1, bool deleteObject=false)
    {
        LeanTween.scale(uiText.gameObject, new Vector3 (endScale, endScale, 1), phaseTime);
        LeanTweenExt.LeanAlphaText(uiText, endOpacity, phaseTime);
        LeanTween.move(uiText.gameObject, uiText.gameObject.transform.position + (offsetDirection*offsetSpeed), phaseTime);
        if (deleteObject)
        { StartCoroutine(DeleteAfterSecs(uiText.gameObject, phaseTime)); }
    }

    public void UIPhaseIn(Image uiImage, float phaseTime, Vector3 offsetDirection, float startScale=1,
                            float startOpacity=0, float endOpacity=1, float offsetSpeed=1, bool deleteObject=false)
    {
        uiImage.transform.localScale = new Vector3(startScale, startScale, 1);
        LeanTween.alpha(uiImage.rectTransform, startOpacity, phaseTime);
        LeanTween.scale(uiImage.gameObject, new Vector3 (1, 1, 1), phaseTime);
        LeanTween.alpha(uiImage.rectTransform, endOpacity, phaseTime);
        LeanTween.move(uiImage.gameObject, uiImage.gameObject.transform.position + (offsetDirection*offsetSpeed), phaseTime);
        if (deleteObject)
        { StartCoroutine(DeleteAfterSecs(uiImage.gameObject, phaseTime)); }
    }

    public void UIPhaseIn(TMP_Text uiText, float phaseTime, Vector3 offsetDirection, float startScale=1,
                            float startOpacity=0, float endOpacity=1, float offsetSpeed=1, bool deleteObject=false)
    {
        uiText.transform.localScale = new Vector3(startScale, startScale, 1);
        Color newColor = uiText.color;
        newColor.a = startOpacity;
        uiText.color = newColor;
        LeanTween.scale(uiText.gameObject, new Vector3 (1, 1, 1), phaseTime);
        LeanTweenExt.LeanAlphaText(uiText, endOpacity, phaseTime);
        LeanTween.move(uiText.gameObject, uiText.gameObject.transform.position + (offsetDirection*offsetSpeed), phaseTime);
        if (deleteObject)
        { StartCoroutine(DeleteAfterSecs(uiText.gameObject, phaseTime)); }
    }

    public void ScaleBoxToText(TMP_Text uiText, Image uiImage, float extraMargin = 10)
    {
        //Find amount of lines text takes then use a multiplier to find the height the box must be for the text size
        uiImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, extraMargin 
                                                    + (uiText.fontSize * uiText.maxVisibleLines));
    }

    private IEnumerator DeleteAfterSecs(GameObject toDelete, float deleteSecs)
    {
        yield return new WaitForSeconds(deleteSecs);
        DestroyImmediate(toDelete);
    }
}