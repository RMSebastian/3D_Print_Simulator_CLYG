using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
[RequireComponent(typeof(PlayerInput))]
[RequireComponent (typeof(AudioSource))]
public class IntroduceFilamentMinigame : MinigameHandler
{
    #region Variables
    [Header("New Parents for list of GO")]
    [SerializeField] protected GameObject leverGO;
    [SerializeField] protected GameObject tubeGO;
    [SerializeField] protected GameObject filamentGO;
    [SerializeField] protected GameObject endFilamentGO;

    [Header("UI Configuration")]
    [SerializeField] protected RectTransform grabberGO;
    [SerializeField] protected RectTransform targetGO;
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected WaitableBase goodAnimtaion;
    [Header("List of GO")]
    [SerializeField] protected List<GameObject> leverGOChildList = new List<GameObject>();
    [SerializeField] protected List<GameObject> filamentGOChildList = new List<GameObject>();
    protected List<GameObject> leverGOParentList = new List<GameObject>();
    protected List<GameObject> filamentGOParentList = new List<GameObject>();
    protected AudioSource source;

    protected Vector3 filamentInitialPosition;
    protected Vector3 imageInitialPosition;
    protected Quaternion levelCloseRotation;
    protected Quaternion levelOpenRotation;

    private bool filamentIntroduce;
    #endregion

    #region Base Configuration
    protected override void Start()
    {
        source = GetComponent<AudioSource>();
        base.Start();
        InitialConfiguration();
    }
    protected void InitialConfiguration()
    {
        canvas.gameObject.SetActive(false);
        filamentInitialPosition = filamentGO.transform.position;
        levelCloseRotation = leverGO.transform.rotation;
        Vector3 eulerAngles = levelOpenRotation.eulerAngles;
        eulerAngles.z -= 20f;
        Quaternion newQuaternion = Quaternion.Euler(eulerAngles);
        leverGO.transform.rotation = newQuaternion;
        levelOpenRotation = leverGO.transform.rotation;
        SetInteractable(false);
        filamentGOChildList[0].SetActive(false);
        filamentIntroduce = false;
    }
    #endregion

    #region Override Methods
    public override void PrepareMinigame()
    {
        StartCoroutine(WaitTimer());
        base.PrepareMinigame();
        SetCameraPosition(0);
        SetInteractable(true);
        SaveParentTransform(ref leverGOChildList, ref leverGOParentList, ref leverGO);
        SaveParentTransform(ref filamentGOChildList, ref filamentGOParentList, ref filamentGO);
        filamentGOChildList[0].SetActive(true);
    }
    public override void FinishedMinigame()
    {

        PlaySound();
        canvas.gameObject.SetActive(false);
        base.FinishedMinigame();
        SetCameraPosition(-1);
        SetInteractable(false);
        SetParentTransform(ref leverGOChildList, ref leverGOParentList);
        SetParentTransform(ref filamentGOChildList, ref filamentGOParentList);
    }
    #endregion

    #region Proper IEnumerators

    #endregion

    #region Interactions
    #region InputSystem Methods
    public void OnDown()
    {
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, filamentGO.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, null, out Vector2 grabberPosition);
        grabberGO.transform.localPosition = grabberPosition;
        imageInitialPosition = grabberPosition;
        screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, tubeGO.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, null, out Vector2 targetPosition);
        targetGO.transform.localPosition = targetPosition;
    }
    public void OnEndDrag()
    {
        if (filamentIntroduce) return;
        SetInteractable(false);
        StartCoroutine(MoveAnimation(filamentGO, filamentInitialPosition, delegate { SetInteractable(true); }));
        StartCoroutine(LocalMoveAnimation(grabberGO.gameObject, imageInitialPosition));
    }
    #endregion

    #region OnMoveEvent Methods
    public void OnDrag(BaseEventData baseEventData)
    {
        if (filamentIntroduce) return;
        PointerEventData eventData = baseEventData as PointerEventData;
        if (eventData.button != PointerEventData.InputButton.Left)return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        grabberGO.transform.localPosition = localPoint;
        Vector2 adjustedLocalPoint = new Vector2(localPoint.x / canvas.scaleFactor,localPoint.y / canvas.scaleFactor);
        Vector3 screenPoint = canvas.transform.TransformPoint(adjustedLocalPoint);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
        worldPoint.z = 0.166f;
        filamentGO.transform.position = worldPoint;
        if (Vector3.Distance(filamentGO.transform.position,endFilamentGO.transform.position) < 0.02f && !filamentIntroduce)
        {
            filamentIntroduce = true;
            filamentGO.transform.position = endFilamentGO.transform.position;
            canvas.gameObject.SetActive(false);
            goodAnimtaion.HandleRequest(FinishedMinigame);
        }

    }
    protected IEnumerator WaitTimer()
    {
        float time = 0;
        while (time < maxTime-0.2f)
        {
            time = (time + Time.deltaTime);
            yield return null;
        }
        yield return null;
        canvas.gameObject.SetActive(true);
        OnDown();
    }
    protected IEnumerator LocalMoveAnimation(GameObject go, Vector3 finalPosition, UnityAction action = null)
    {
        float time = 0;
        Vector3 initialPosition = go.transform.localPosition;
        while (time < maxTime)
        {
            go.transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, curve.Evaluate(time / maxTime));
            time = (time + Time.deltaTime);
            yield return null;
        }
        yield return null;
        go.transform.localPosition = finalPosition;

    }
    #endregion
    #endregion
    public void PlaySound()
    {
        source.Play();
    }
}
