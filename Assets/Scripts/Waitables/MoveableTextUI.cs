using TMPro;
using UnityEngine.Events;
using UnityEngine;
using System.Security.Cryptography;
using System.Collections;
public class MoveableTextUI : WaitableBase
{
    [Header("UI References")]
    [SerializeField] protected TextMeshProUGUI txtLabel;
    [SerializeField] protected RectTransform pnlLabel;

    [Header("Configurations")]
    [SerializeField] protected string textString = " ";
    [SerializeField] protected float appearTime = 1;
    [SerializeField] protected float disappearTime = 1;
    [SerializeField] protected AnimationCurve curve;

    protected Vector3 beginPosition;
    protected Vector3 finalPosition;
    private void Awake()
    {
        beginPosition.y = (Screen.height / 2);
        beginPosition.x = (Screen.width / 2);
        finalPosition.x = (Screen.width / 2);
        finalPosition.y = -(Screen.height / 2);
    }
    [ContextMenu("Test")]
    public void Test() => HandleRequest();
    public override void HandleRequest(UnityAction finalRequest = null)
    {
        txtLabel.text = textString;
        UnityAction nextAction = () => 
        {

            StartCoroutine
            (
                MoveAnimation
                (
                    pnlLabel.gameObject,
                    beginPosition,
                    finalPosition,
                    appearTime,
                    () => base.HandleRequest(finalRequest)  
                )
            );
        };
        StartCoroutine
            (
                MoveAnimation
                (
                    pnlLabel.gameObject,
                    finalPosition,
                    beginPosition,
                    appearTime,
                    nextAction
                )
            );
    }

    protected IEnumerator MoveAnimation(GameObject go,Vector3 initialPosition, Vector3 finalPosition,float maxTime, UnityAction action = null)
    {
        float time = 0;
        while (time < maxTime)
        {
            time = (time + Time.deltaTime);
            go.transform.position = Vector3.Lerp(initialPosition, finalPosition, curve.Evaluate(time / maxTime));
            yield return null;
        }
        yield return null;
        go.transform.position = finalPosition;
        action?.Invoke();
    }
}
