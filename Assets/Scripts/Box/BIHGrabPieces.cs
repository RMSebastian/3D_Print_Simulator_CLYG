using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BIHGrabPieces : BoxInteractionHandler
{
    [SerializeField] private int BIHindex;
    [SerializeField] private Animator GrabUIAnimation;
    [SerializeField] private Collider boxCollider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject PanelGO;
    [SerializeField] private UnityEvent uEvent;

    public override void HandlerRequest(int index, UnityAction action)
    {
        if (index != BIHindex)
        {
            base.HandlerRequest(index, action);
        }
        else
        {
            GrabUIAnimation.SetTrigger("Grab");
            text.text = "PIEZAS AGREGADAS AL INVENTARIO EN EL MANUAL";
            PanelGO.SetActive(false);
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
        uEvent.Invoke();
    }
}
