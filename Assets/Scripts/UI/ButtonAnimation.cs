using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IButtonActivation
{
    [SerializeField] private Button btn;
    public void Animating(UnityAction action)
    {
        StartCoroutine(Activating(action));
    }
    private IEnumerator Activating(UnityAction action = null)
    {
        yield return new WaitForSeconds(1f);
        btn.enabled = true;
        btn.interactable = true;
        yield return new WaitForSeconds(.5f);
        btn.onClick.Invoke();
        yield return new WaitForSeconds(1.5f);
        if(action != null) action.Invoke();
    }
}
