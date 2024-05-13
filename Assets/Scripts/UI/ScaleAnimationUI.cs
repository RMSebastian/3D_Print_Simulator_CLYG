using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScaleAnimationUI : MonoBehaviour
{
    [SerializeField] protected float interceptDistance = 1f;
    [SerializeField] protected float amplitudeMultiplier = 0.25f;
    protected float frequencyMultiplier = 1.5f;
    private float scaleFactor = 1.0f;
    private Image imageComponent;
    private Vector3 originalScale;
    private void OnDisable()
    {
        this.transform.localScale = originalScale;
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        originalScale = transform.localScale;
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
            scaleFactor = amplitudeMultiplier * Mathf.Cos(Time.time * frequencyMultiplier) + interceptDistance;
            imageComponent.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            yield return null;
        }
    }
}
