using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AudioSource))]
public class SlicerSoftwareTest : TestHandler
{
    #region Variables
    [SerializeField] protected RectTransform slicerPanel;
    [SerializeField] protected TextMeshProUGUI txtTimeSlice;
    [SerializeField] protected TextMeshProUGUI txtFilamentSlice;
    [SerializeField] protected TextMeshProUGUI txtButtonSlice;
    [SerializeField] protected Image imgButtonSlice;
    [SerializeField] protected Button btnSlice;
    [SerializeField] protected Button btnOpenFolder;
    [SerializeField] protected TMP_Dropdown drpQualityList;
    [SerializeField] protected GameObject floorGO;
    [SerializeField] protected MaterialManager modelGO;
    [SerializeField] protected GameObject SDGO;
    [SerializeField] protected Transform SDEmbedPosition;
    [SerializeField] protected Transform SDLayPosition;

    protected bool canRotate;
    protected bool segmented;

    protected AudioSource source;
    protected WaitForSeconds bfmTime = new WaitForSeconds(0.2f);
    #endregion
    #region Basic Configuration
    protected override void Start()
    {
        source = GetComponent<AudioSource>();
        base.Start();
        InitialConfiguration();
    }
    protected void InitialConfiguration()
    {
        SetInteractable(false);
        modelGO.gameObject.SetActive(false);
        SDGO.SetActive(false);
        btnSlice.interactable = false;
        btnOpenFolder.interactable = false;
        drpQualityList.interactable |= false;
        slicerPanel.sizeDelta = new Vector2 (slicerPanel.sizeDelta.x, 7);
        txtFilamentSlice?.transform.parent.gameObject.SetActive(false);
        txtTimeSlice?.transform.parent.gameObject.SetActive(false);
    }
    #endregion
    #region Override Methods
    public override void PrepareMinigame()
    {
        SDGO.SetActive(true);
        maxTime = 0.25f;
        source.Play();
        StartCoroutine(MoveAnimation(SDGO,SDEmbedPosition.position,()=>
        {
            maxTime = 0.95f;
            btnOpenFolder.interactable = true;
            drpQualityList.interactable |= true;
            SetCameraPosition(0);
            SetInteractable(true);
        }));
        base.PrepareMinigame();
    }
    public override void FinishedMinigame()
    {
        btnSlice.interactable = false;
        btnOpenFolder.interactable = false;
        drpQualityList.interactable |= false;
        SetCameraPosition(-1);
        SetInteractable(false);
        base.FinishedMinigame();
        maxTime = 0.25f;
        source.Play();
        StartCoroutine(MoveAnimation(SDGO, SDLayPosition.position, () => SDGO.SetActive(false)));
    }

    #endregion
    #region InputSystem Methods
    public void OnSelect(InputAction.CallbackContext ctx)
    {
        if (!canInteract) return;
        if (ctx.started) canRotate = true;
        else if(ctx.canceled) canRotate = false;
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canRotate)
        {
            float yAxis = Mathf.Clamp(ctx.ReadValue<Vector2>().x, 0.005f, -0.005f);
            RotateFloor(new Vector3(0, ctx.ReadValue<Vector2>().x, 0));
        }
    }
    private void RotateFloor(Vector3 rotation)
    {
        rotation *= (Time.deltaTime*2);
        floorGO?.transform.Rotate(rotation, Space.World);
    }
    #endregion
    public void OnSelectModel()
    {
        btnSlice.interactable = true;
        btnOpenFolder.interactable = false;
        modelGO.gameObject.SetActive(true); 
    }
    public void OnClick()
    {
        if(!segmented)
        {
            StartCoroutine(LoadImage());
        }
        else
        {
            FinishedMinigame();
        }
        
    }
    protected IEnumerator LoadImage()
    {
        float time = 0;
        float maxTimer = 1;
        
        btnSlice.interactable = false;
        btnOpenFolder.interactable = false;
        drpQualityList.interactable = false;
        Color defaultcolor = imgButtonSlice.color;
        imgButtonSlice.color = Color.green;
        txtButtonSlice.text = "SEGMENTANDO...";
        while (time < maxTimer)
        {
            imgButtonSlice.fillAmount = Mathf.Lerp(0, 1, time/maxTimer);
            time = (time + Time.deltaTime);
            yield return null;
        }
        yield return null;
        imgButtonSlice.color = defaultcolor;
        imgButtonSlice.fillAmount = 1;
        txtButtonSlice.text = "GUARDAR EN MEMORIA SD";
        slicerPanel.sizeDelta = new Vector2(slicerPanel.sizeDelta.x, 14);
        switch (drpQualityList.value)
        {
            case 0:
                txtFilamentSlice.text = "30g - 10,26m";
                txtTimeSlice.text = "1 Hora 30 minutos";
                break;
            case 1:
                txtFilamentSlice.text = "15g - 8,26m";
                txtTimeSlice.text = "45 minutos";
                break;
            case 2:
                txtFilamentSlice.text = "7g - 4,13m";
                txtTimeSlice.text = "15 minutos";
                break;
            default:
                break;
        }
        modelGO.ChangeColor(Color.yellow);
        txtFilamentSlice?.transform.parent.gameObject.SetActive(true);
        txtTimeSlice?.transform.parent.gameObject.SetActive(true);
        segmented = true;
        btnSlice.interactable = true;

    }
}
