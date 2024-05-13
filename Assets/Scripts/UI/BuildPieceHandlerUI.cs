using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildPieceHandlerUI : MonoBehaviour
{
    public int ID;
    private UnityAction<int, GameObject> action;

    public void SubscribeToAction(UnityAction<int, GameObject> action) => this.action += action;

    public void InvokeAction() => this.action.Invoke(ID, this.gameObject);
}
