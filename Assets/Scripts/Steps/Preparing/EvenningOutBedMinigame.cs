using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class EvenningOutBedMinigame : MinigameHandler
{

    #region Variables
    [Header("UI References")]
    [SerializeField] protected Slider sldRotator;
    [SerializeField] protected WaitableBase goodAnimation;
    [SerializeField] protected WaitableBase soundAnimation;
    [Header("New Parents for list of GO")]
    [SerializeField] protected GameObject GuideGO;
    [SerializeField] protected GameObject hotendGO;
    [SerializeField] protected GameObject valveGO;
    [SerializeField] protected GameObject paperGO;
    
    protected GameObject currentValveGO;
    protected AudioSource source;

    protected Material paperMaterial;
    private LayerMask minigameMask = 7;

    [Header("List of GO")]
    [SerializeField] protected List<GameObject> valveGOChildList = new List<GameObject>();
    [SerializeField] protected List<GameObject> GuideGOChildList = new List<GameObject>();
    [SerializeField] protected List<GameObject> hotendGOChildList = new List<GameObject>();
    protected List<GameObject> valveGOParentList = new List<GameObject>();
    protected List<GameObject> hotendGOParentList = new List<GameObject>();
    protected List<GameObject> GuideGOParentList = new List<GameObject>();

    protected int valveIndex = 0;

    [Header("HotendPosition Data")]
    [SerializeField] protected float xDirection = 0.385f;
    [SerializeField] protected float yDirection = -0.96f;
    [SerializeField] protected float yMaxHeight = -0.94f;
    [SerializeField] protected float yMinHeight = -0.98f;
    
    protected bool passing;

    protected float clipSize;
    protected WaitForSecondsRealtime time;
    private bool isRotating;
    #endregion

    #region Basic Configuration
    protected override void Start()
    {
        source = GetComponent<AudioSource>();
        clipSize = source.clip.length;
        time = new WaitForSecondsRealtime(clipSize);
        base.Start();
        InitialConfiguration();
    }
    protected void InitialConfiguration()
    {
        sldRotator.gameObject.SetActive(false);
        paperMaterial = paperGO.GetComponent<MeshRenderer>().material;
        SetInteractable(false);
        paperGO.SetActive(false);
    }
    #endregion

    #region OverrideMethods
    public override void PrepareMinigame()
    {
        base.PrepareMinigame();
        SetInteractable(true);
        sldRotator.gameObject.SetActive(false);
        currentValveGO = valveGOChildList[valveIndex];
        SaveParentTransform(ref valveGOChildList,ref valveGOParentList,ref valveGO);
        SaveParentTransform(ref GuideGOChildList, ref GuideGOParentList, ref GuideGO);
        SaveParentTransform(ref hotendGOChildList, ref hotendGOParentList, ref hotendGO);
        SetObjectsPosition();
    }
    public override void FinishedMinigame()
    {
        base.FinishedMinigame();


        SetInteractable(false);
        sldRotator.gameObject.SetActive(false);
        StartCoroutine(MoveAnimation(GuideGO, new Vector3(0, 0, 0),delegate {SetParentTransform(ref GuideGOChildList, ref GuideGOParentList);}));
        StartCoroutine(MoveAnimation(hotendGO, new Vector3(0, 0, 0), delegate {SetParentTransform(ref hotendGOChildList, ref hotendGOParentList); SetCameraPosition(-1); }));
        SetParentTransform(ref valveGOChildList, ref valveGOParentList);
        paperGO.SetActive(false);
    }
    #endregion

    #region SetHotendPosition Methods
    protected void NextPoint()
    {

        if (valveIndex >= valveGOChildList.Count - 1)
        {
            FinishedMinigame();
            goodAnimation.HandleRequest();
        }
        else
        {
            soundAnimation.HandleRequest();
            valveIndex++;
            currentValveGO = valveGOChildList[valveIndex];
            SetObjectsPosition();
            passing = false;
        }
        
    }
    protected void SetObjectsPosition()
    {
        SetCameraPosition(valveIndex);
        float yCurrentDirection;
        float sldDirection;
        if (Random.Range(0, 2) == 0)
        {
            sldDirection = 0;
            yCurrentDirection = yMinHeight;
        }
        else
        {
            sldDirection = 1;
            yCurrentDirection = yMaxHeight;
        }
        float xCurrentDirection = (valveIndex % 2 == 0) ?xDirection:-xDirection;

        sldRotator.gameObject.SetActive(false);
        paperGO.gameObject.SetActive(false);
        StartCoroutine(MoveAnimation(GuideGO,new Vector3(0, yCurrentDirection, 0),()=> { sldRotator.gameObject.SetActive(true); sldRotator.value = sldDirection; paperGO.gameObject.SetActive(true); }));
        StartCoroutine(MoveAnimation(hotendGO, new Vector3(xCurrentDirection, yCurrentDirection, 0)));
        StartCoroutine(MoveAnimation(paperGO, new Vector3(valveGOChildList[valveIndex].transform.position.x, paperGO.transform.position.y, valveGOChildList[valveIndex].transform.position.z)));
        ChangerPaperColor(yCurrentDirection);
    }

    #endregion

    #region Interactions
    #region InputSystem Methods
    public void MovingValve()
    {
        float yAxis = Mathf.Lerp(yMinHeight, yMaxHeight, sldRotator.value);
        RotateValve(new Vector3(0, yAxis,0));
        RaiseHotend(new Vector3(0, yAxis, 0));
    }
    #endregion

    #region OnMoveEvent Methods
    private void RotateValve(Vector3 rotation)
    {
        currentValveGO.transform.Rotate(rotation,Space.World);
        if (!isRotating)
        {
            StartCoroutine(RotatingSound());
        }
    }
    private IEnumerator RotatingSound()
    {
        isRotating = true;
        //source.Play();
        yield return time;
        isRotating = false;
    }
    private void RaiseHotend(Vector3 position)
    {
        GuideGO.transform.position = position;
        ChangerPaperColor(GuideGO.transform.position.y);
    }
    protected void ChangerPaperColor(float yPosition)
    {
        float percentage;
        if (yPosition > yDirection)
        {
            percentage = ((yPosition - (yDirection)) / ((yMaxHeight) - (yDirection)));
        }
        else
        {
            percentage = ((yPosition - (yDirection)) / ((yMinHeight) - (yDirection)));
        }
        
        paperMaterial.color = Color.Lerp(Color.green, Color.red, percentage);
        if (percentage <= 0.1f && !passing)
        {
            passing = true;
            NextPoint();
        }
    }
    #endregion
    #endregion
}
