using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{
    [SerializeField] private ProgressBarHandler progressBarHandler;

    public void Init(int ID, int value) => progressBarHandler.Init(ID, value);
    public void AddValue() => progressBarHandler.AddValue();
    public void  NextSlider() => progressBarHandler.SelectNextSlider();
}
