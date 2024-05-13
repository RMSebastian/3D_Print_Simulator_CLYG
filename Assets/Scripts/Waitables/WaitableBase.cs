using UnityEngine;
using UnityEngine.Events;

public abstract class WaitableBase : MonoBehaviour
{
    public WaitableBase succesor;

    public virtual void HandleRequest(UnityAction finalRequest = null)
    {
        if (succesor != null)
        {
            succesor.HandleRequest(finalRequest);
        }
        else
        {
            finalRequest?.Invoke();
        }
    }
}
