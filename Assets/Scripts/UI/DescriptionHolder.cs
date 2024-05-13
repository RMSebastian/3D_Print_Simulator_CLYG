using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtDescription;

    public void SetDescription(string description)
    {
        txtDescription.text = description;
    }
}
