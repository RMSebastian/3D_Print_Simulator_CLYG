using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MinigameHandler : MinigameManager
{
    #region Variables
    [Header("Camera Options")]
    public List<GameObject> cameraPosition = new List<GameObject>();
    public float cameraOrtographicSize = 0.1f;
    protected CameraHandler cameraHandler;
    protected Camera cam;
    [Header("MoveAnimation Configuration")]
    [SerializeField] protected AnimationCurve curve;
    [SerializeField] protected float maxTime = 0.95f;

    protected bool canInteract;
    protected bool firstTime;
    #endregion

    #region Basic Configuration
    protected override void Start()
    {
        base.Start();
        cam = Camera.main;
        cameraHandler = Camera.main.GetComponent<CameraHandler>();
    }
    public void SetInteractable(bool interactable) => canInteract = interactable;
    #endregion
    
    #region CameraSet
    protected void SetCameraPosition(int index)
    {
        if(index == -1)
        {
            cameraHandler.ActivateMainCamera();
            return;
        }
        if(!firstTime)
        {
            firstTime = true;
            cameraHandler.SetMinigameCameraTransform(cameraPosition[index].transform.position, cameraPosition[index].transform.rotation, cameraOrtographicSize);
            cameraHandler.ActivateMinigameCamera();
        }
        else
        {
            cameraHandler.SetMinigameCameraMovement(cameraPosition[index].transform.position, cameraPosition[index].transform.rotation, cameraOrtographicSize);
        }
    }
    protected void ConstantlyMoveCamera(Vector3 position)
    {
        cameraHandler.SetMinigameCameraTransformOffset(position);
    }
    #endregion
    #region Animations

    protected IEnumerator MoveAnimation(GameObject go, Vector3 finalPosition, UnityAction action = null)
    {
        float time = 0;
        Vector3 initialPosition = go.transform.position;
        while (time < maxTime)
        {
            go.transform.position = Vector3.Lerp(initialPosition, finalPosition, curve.Evaluate(time / maxTime));
            time = (time + Time.deltaTime);
            yield return null;
        }
        yield return null;
        go.transform.position = finalPosition;
        if (action != null)
        {
            action.Invoke();
        }
    }

    #endregion

    #region Save/Set GOParents On Lists
    protected void SaveParentTransform(ref List<GameObject> childGoList, ref List<GameObject> parentGoList,ref GameObject newParent)
    {
        foreach (GameObject go in childGoList)
        {
            parentGoList.Add(go.transform.parent.gameObject);
            go.transform.SetParent(newParent.transform);
        }
    }
    protected void SetParentTransform(ref List<GameObject> childGoList, ref List<GameObject> parentGoList)
    {
        int i = 0;
        foreach (GameObject go in childGoList)
        {
            go.transform.SetParent(parentGoList[i].transform);
            i++;
        }
    }
    #endregion


}
