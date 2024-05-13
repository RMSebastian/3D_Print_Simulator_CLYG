using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BIHGrabMessage : BoxInteractionHandler
{
    [SerializeField] private int BIHindex;
    [SerializeField] private GameObject messageGO;
    [SerializeField] private GameObject messageUI;
    [SerializeField] private Button passButton;
    private void Start()
    {
        messageGO.gameObject.SetActive(true);
        messageUI.gameObject.SetActive(false);
    }
    public override void HandlerRequest(int index, UnityAction action)
    {
        if(index != BIHindex)
        {
            base.HandlerRequest(index, action);
        }
        else
        {
            messageUI.gameObject.SetActive(true);
            messageGO.gameObject.SetActive(false);
        }
    }
}
