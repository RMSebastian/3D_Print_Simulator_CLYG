using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGroupAnimationUI : MonoBehaviour
{
    protected float interceptDistance = 0.5f;
    protected float amplitudeMultiplier = 0.5f;
    protected float frequencyMultiplier = 1.75f;
    private float scaleFactor = 0.5f;
    private Image imageComponent;

    private void OnDisable()
    {
        imageComponent.color = Color.clear;
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        
        StartCoroutine(ScaleImage());
    }
    private void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    private IEnumerator ScaleImage()
    {
        while (true)
        {
            scaleFactor = amplitudeMultiplier * Mathf.Sin(Time.time * frequencyMultiplier) + interceptDistance;
            Color selectedColor = Color.Lerp(Color.white, Color.clear, scaleFactor);
            imageComponent.color = selectedColor;
            yield return null;
        }
    }
}
