using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class FinalAnimationScript : MonoBehaviour
{
    [Header("List of GO")]
    [SerializeField] protected List<GameObject> GuideGOChildList = new List<GameObject>();
    [SerializeField] protected List<GameObject> hotendGOChildList = new List<GameObject>();
    protected List<GameObject> hotendGOParentList = new List<GameObject>();
    protected List<GameObject> GuideGOParentList = new List<GameObject>();
    [Header("New Parents for list of GO")]
    [SerializeField] protected GameObject GuideGO;
    [SerializeField] protected GameObject hotendGO;
    [SerializeField] protected GameObject printableGO;
    [Header("Configuration")]
    [SerializeField] protected float interceptDistance = 50f;
    [SerializeField] protected float amplitudeMultiplier = 50f;
    [SerializeField] protected float frequencyMultiplier = 2.5f;
    [Header("Camera Options")]
    public List<GameObject> cameraPosition = new List<GameObject>();
    public float cameraOrtographicSize = 0.1f;
    protected CameraHandler cameraHandler;
    protected Camera cam;
    [Header("Final Events")]
    [SerializeField] protected UnityEvent finalEvents;
    [SerializeField] protected UnityEvent initialEvents;
    [SerializeField] protected UnityEvent beginEvents;
    [SerializeField] protected UnityEvent endEvets;
    protected float Yoffset = -1.56f;
    protected bool started;
    protected bool firstTime;
    protected float maxTime = 1;
    protected float high;
    private void Start()
    {
        GameManager.Instance.SetFinalInteraction(this);
        cam = Camera.main;
        cameraHandler = Camera.main.GetComponent<CameraHandler>();
        for (int i = 0; i < printableGO.transform.childCount; i++)
        {
            printableGO.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void PrepareMinigame()
    {
        SaveParentTransform(ref GuideGOChildList, ref GuideGOParentList, ref GuideGO);
        SaveParentTransform(ref hotendGOChildList, ref hotendGOParentList, ref hotendGO);
        SetObjectsPosition();
        SetCameraPosition(0);
        initialEvents?.Invoke();
    }
    public void FinishedMinigame()
    {
        endEvets?.Invoke();
        StartCoroutine(MoveAnimation(GuideGO, new Vector3(0, 0, 0), delegate { SetParentTransform(ref GuideGOChildList, ref GuideGOParentList); finalEvents?.Invoke(); }));
        StartCoroutine(MoveAnimation(hotendGO, new Vector3(0, 0, 0), delegate { SetParentTransform(ref hotendGOChildList, ref hotendGOParentList); SetCameraPosition(-1); }));
        
    }
    protected void SetObjectsPosition()
    {
        float yCurrentDirection =printableGO.transform.GetChild(0).TransformPoint(Vector3.zero).y +Yoffset;
        StartCoroutine(MoveAnimation(GuideGO, new Vector3(0, yCurrentDirection, 0), delegate { StartCoroutine(MoverCadaCincoSegundos()); beginEvents?.Invoke(); }));
    }
    protected IEnumerator MoveAnimation(GameObject go, Vector3 finalPosition, UnityAction action = null)
    {
        float time = 0;
        Vector3 initialPosition = go.transform.position;
        while (time < maxTime)
        {
            go.transform.position = Vector3.Lerp(initialPosition, finalPosition,time / maxTime);
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
    #region Save/Set GOParents On Lists
    protected void SaveParentTransform(ref List<GameObject> childGoList, ref List<GameObject> parentGoList, ref GameObject newParent)
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

    private void Update()
    {
        if (!started) return;
        float direction = (amplitudeMultiplier * Mathf.Sin(Time.time * frequencyMultiplier) + interceptDistance);
        hotendGO.transform.position = new Vector3(direction, hotendGO.transform.position.y, hotendGO.transform.position.z);
        GuideGO.transform.position = new Vector3(GuideGO.transform.position.x, high, GuideGO.transform.position.z);
    }

    private IEnumerator MoverCadaCincoSegundos()
    {
        started = true;
        int index = 0;

        while (index < printableGO.transform.childCount - 1)
        {
            Transform child = printableGO.transform.GetChild(index);

            Vector3 worldPosition = child.TransformPoint(Vector3.zero);

            high = worldPosition.y + Yoffset;

            printableGO.transform.GetChild(index).gameObject.SetActive(true);
            
            index++;

            yield return new WaitForSeconds(1f);
        }
        started = false;

        printableGO.transform.GetChild(printableGO.transform.childCount - 1).gameObject.SetActive(true);
        
        FinishedMinigame();
    }
    protected void SetCameraPosition(int index)
    {
        if (index == -1)
        {
            cameraHandler.ActivateMainCamera();
            return;
        }
        if (!firstTime)
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
    #endregion
}
