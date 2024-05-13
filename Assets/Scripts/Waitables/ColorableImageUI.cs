using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class ColorableImageUI : WaitableBase
{
    [Header("UI References")]
    [SerializeField] protected Image imgForeground;
    [SerializeField] protected Color finalColor;

    [Header("Configurations")]
    [SerializeField] protected float blinkTime = 0.05f;
    [SerializeField] protected float maxTime = 0.5f;

    protected CanvasGroup canvasGroup;
    protected Color beginColor;
    private void Start()
    {
        beginColor = imgForeground.color;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }
    public override void HandleRequest(UnityAction finalRequest = null)
    {
        StartCoroutine(BlinkAnimation(() => base.HandleRequest(finalRequest)));
    }

    private IEnumerator BlinkAnimation(UnityAction action = null)
    {
        canvasGroup.blocksRaycasts = true;
        float secs = 0;
        bool red = false;
        while (secs < maxTime)
        {
            imgForeground.color = red ? beginColor : finalColor;
            red = !red;
            secs += blinkTime;
            yield return new WaitForSeconds(blinkTime);
        }
        imgForeground.color = beginColor;
        action?.Invoke();
        canvasGroup.blocksRaycasts = false;
    }
}
