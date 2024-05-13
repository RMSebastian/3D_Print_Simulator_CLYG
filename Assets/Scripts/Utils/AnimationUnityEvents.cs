using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationUnityEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent onBeginEvent;
    [SerializeField] private UnityEvent onEndEvent;
    public void AddOnBeginEvent(UnityAction action) => onBeginEvent.AddListener(action);
    public void AddOnEndEvent(UnityAction action) => onEndEvent.AddListener(action);
    public void ResetOnBeginEvent()=> onBeginEvent.RemoveAllListeners();
    public void ResetOnEndEvent()=> onEndEvent.RemoveAllListeners();
    public void BeginEventInvoke()=>onBeginEvent?.Invoke();
    public void EndEventInvoke()=>onEndEvent?.Invoke();
}
