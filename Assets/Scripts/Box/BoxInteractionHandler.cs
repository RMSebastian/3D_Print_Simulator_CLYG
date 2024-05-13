using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BoxInteractionHandler : MonoBehaviour
{
    [SerializeField] protected BoxInteractionHandler succesor;
    public virtual void HandlerRequest(int index, UnityAction action)
    {
        if(succesor != null)
        {
            succesor.HandlerRequest(index, action);
        }
    }
}
