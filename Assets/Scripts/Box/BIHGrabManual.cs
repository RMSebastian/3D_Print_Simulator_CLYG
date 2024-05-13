using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BIHGrabManual : BoxInteractionHandler
{
    [SerializeField] private int BIHindex;
    [SerializeField] private GameObject ManualGO;
    [SerializeField] private GameObject ManualUI;
    [SerializeField] private Animator GrabUIAnimation;
    [SerializeField] private Collider boxCollider;
    [SerializeField] private TextMeshProUGUI text;
    private void Start()
    {
        ManualGO.SetActive(true);
        ManualUI.SetActive(false);
    }
    public override void HandlerRequest(int index, UnityAction action)
    {
        if (index != BIHindex)
        {
            base.HandlerRequest(index, action);
        }
        else
        {
            ManualGO.SetActive(false);
            ManualUI.SetActive(true);
            text.text = "MANUAL AGARRADO";
            GrabUIAnimation.SetTrigger("Grab");
            StartCoroutine(DisableBoxCollider());
        }
    }
    private IEnumerator DisableBoxCollider()
    {
        yield return null;
        boxCollider.enabled = false;
        float animLenght = GrabUIAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animLenght);
        boxCollider.enabled = true;
    }
}
