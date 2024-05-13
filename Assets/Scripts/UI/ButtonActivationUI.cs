using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivationUI : MonoBehaviour
{
    [SerializeField] private GameObject pnlText;
    public void ActivatedChildButtons(bool active)
    {
        foreach (Button btn in this.transform.GetComponentsInChildren<Button>())
        {
            btn.interactable = active;
        }
        pnlText.SetActive(!active);
    }
}
