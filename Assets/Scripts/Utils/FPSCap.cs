using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCap : MonoBehaviour
{
    [SerializeField] private int frames = 30;
    // Start is called before the first frame update
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frames;
    }
}
