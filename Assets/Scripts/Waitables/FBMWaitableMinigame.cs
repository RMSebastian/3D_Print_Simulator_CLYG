using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FBMWaitableMinigame : WaitableBase
{
    [SerializeField] protected GameObject buttons;
    [SerializeField] protected WaitableBase goodAnimation;
    [SerializeField] protected UnityEvent BeginEvents;
    [SerializeField] protected UnityEvent EndEvents;

    protected GameObject currentGO;
    private PositionsLibrary positions;
    private Quaternion finalRotation;
    protected UnityAction action;

    protected int index = 1;
    protected float maxTime = 0.5f;
    protected bool rotating;
    public override void HandleRequest(UnityAction finalRequest = null)
    {
        action = finalRequest;
        buttons.SetActive(true);
        rotating = false;
        BeginEvents?.Invoke();
    }

    void FinishMinigame()
    {
        buttons.SetActive(false);
        if (action != null)
            action.Invoke();
        EndEvents?.Invoke();
    }

    public void BeginMinigame(GameObject go, PositionsLibrary positions, Quaternion finalRotation)
    {
        currentGO= go;
        this.positions = positions;
        this.finalRotation = finalRotation;

        foreach(Transform child in positions.transforms) if(go.transform.position ==  child.position) 
            { index = child.GetSiblingIndex(); }
    }
    public void Rotate(int i)
    {
        if (rotating) return;
       
        rotating = true;

        if (index + i == positions.transforms.Count) index = 0;
        else if (index + i == -1) index = positions.transforms.Count - 1;
        else index += i;

        StartCoroutine(StartMovement(currentGO, currentGO.transform, positions.transforms[index]));


    }
    protected virtual IEnumerator StartMovement(GameObject go, Transform begin, Transform end)
    {
        float time = 0;
        while (time < maxTime)
        {
            go.transform.position = Vector3.Lerp(begin.position, end.position, time/maxTime);
            go.transform.rotation = Quaternion.Lerp(begin.rotation, end.rotation, time/maxTime *1.1f);
            time = (time + Time.deltaTime);
            yield return null;
        }
        go.transform.position=end.position;
        go.transform.rotation=end.rotation;
        yield return null;

        if(currentGO.transform.rotation == finalRotation)
        {
            goodAnimation.HandleRequest(FinishMinigame);

        }
        else
        {
            rotating = false;
        }

    }
}
