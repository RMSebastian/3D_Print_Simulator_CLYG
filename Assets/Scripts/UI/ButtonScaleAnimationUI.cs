using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScaleAnimationUI : MonoBehaviour
{
    [SerializeField] protected float interceptDistance = 1f;
    [SerializeField] protected float amplitudeMultiplier = 0.1f;
    protected float frequencyMultiplier = 2f;
    private float scaleFactor = 1.0f;
    private Image imageComponent;
    private Button buttonComponent;
    private void OnDisable()
    {
        imageComponent.transform.localScale = Vector3.one;
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        StartCoroutine(ScaleImage());
    }
    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        buttonComponent = GetComponent<Button>();
    }

    private IEnumerator ScaleImage()
    {
        while (true)
        {
            if(buttonComponent.interactable)
            {
                scaleFactor = amplitudeMultiplier * Mathf.Cos(Time.time * frequencyMultiplier) + interceptDistance;
                imageComponent.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }
            yield return null;
        }
    }
}
