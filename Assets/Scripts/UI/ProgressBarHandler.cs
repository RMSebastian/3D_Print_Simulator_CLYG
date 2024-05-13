using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarHandler : MonoBehaviour
{
    [SerializeField] private List<Slider> progressSlider = new List<Slider>();
    [SerializeField] private List<Image> images = new List<Image>();
    [SerializeField] private RectTransform imageParent;
    [SerializeField] private Color color;
    private int listIndex = -1;
    private void Start()
    {
        images[0].color = color;
    }
    public void Init(int ID, int totalValue)
    {
        progressSlider[ID].maxValue = totalValue;
    }
    public void AddValue()
    {
        progressSlider[listIndex].value += 1;
    }
    public void SelectNextSlider()
    {
        if (listIndex + 1 > progressSlider.Count - 1)
        {
            images[images.Count-1].color = color;
        }
        listIndex++;
        if (images[listIndex] !=null)
        {
            images[listIndex].color = color;
        }
    }

}
