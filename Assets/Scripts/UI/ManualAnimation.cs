using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ManualAnimation : MonoBehaviour
{
    //Deberia usar un IManualAnimation?
    [Header("UI references")]
    [SerializeField] private GameObject pnlManualMenu;
    [SerializeField] private GameObject pnlBase;
    [SerializeField] private CanvasGroup cgrGeneralButtons;
    [SerializeField] private CanvasGroup cgrStepButtons;
    [SerializeField] private ButtonsStepManager btnManager;
    [SerializeField] private UnityEvent onClosedManualEvent;
    [SerializeField] private UnityEvent onOpenManualEvent;
    [SerializeField] private UnityEvent onNextStepEndsEvent;
    [SerializeField] private UnityEvent onNextStepBeginEvent;
    private float currentHeight;
    private float referenceHeight = 1080;
    private float yMultiplier;
    private void Start()
    {
        currentHeight = Screen.height;
        yMultiplier = currentHeight / referenceHeight;
    }
    public void NextStep()
    {
        onNextStepEndsEvent?.Invoke();
        StartCoroutine(MoveUpAnimation(pnlBase, 0.9f, (()=>btnManager.ActivateNextButton(()=>StartCoroutine(MoveDownAnimation(pnlBase,0.9f, delegate { onNextStepBeginEvent?.Invoke(); }))))));
    }
    public void OpenManualAnimation()
    {
        StartCoroutine(MoveUpAnimation(pnlBase,0.25f));
        onOpenManualEvent?.Invoke();
    }
    public void CloseManualAnimation()
    {
        StartCoroutine(MoveDownAnimation(pnlBase, 0.25f, (()=> onClosedManualEvent?.Invoke())));
    }
    protected IEnumerator MoveUpAnimation(GameObject manualGO,float maxTime, UnityAction nextCall = null)
    {
        cgrGeneralButtons.blocksRaycasts = false;
        cgrGeneralButtons.alpha = 0;
        cgrStepButtons.blocksRaycasts = false;
        float time = 0;
        Vector3 initialPosition = new Vector3(manualGO.transform.position.x, -((currentHeight) * yMultiplier), manualGO.transform.position.z);
        Vector3 finalPosition = manualGO.transform.position;
        while (time < maxTime)
        {
            manualGO.transform.position = Vector3.Lerp(initialPosition, finalPosition, time/maxTime);
            time += Time.deltaTime;
            yield return null;
        }
        manualGO.transform.position = finalPosition;
        pnlManualMenu.SetActive(true);
        if(nextCall == null)
        {
            cgrGeneralButtons.blocksRaycasts = true;
            cgrGeneralButtons.alpha = 1;
            cgrStepButtons.blocksRaycasts = true;
        }
        else
        {
            nextCall.Invoke();
        }
    }
    protected IEnumerator MoveDownAnimation(GameObject manualGO, float maxTime, UnityAction nextCall = null)
    {
        cgrGeneralButtons.blocksRaycasts = false;
        cgrGeneralButtons.alpha = 0;
        cgrStepButtons.blocksRaycasts = false;
        float time = 0;
        Vector3 initialPosition = manualGO.transform.position;
        Vector3 finalPosition = new Vector3(manualGO.transform.position.x, -((currentHeight) * yMultiplier), manualGO.transform.position.z);
        while (time < maxTime)
        {
            manualGO.transform.position = Vector3.Lerp(initialPosition, finalPosition, time / maxTime);
            time = (time + Time.deltaTime);
            yield return null;
        }
        manualGO.transform.position = initialPosition;
        cgrGeneralButtons.blocksRaycasts = true;
        cgrGeneralButtons.alpha = 1;
        cgrStepButtons.blocksRaycasts = true;
        pnlManualMenu.SetActive(false);
       if(nextCall != null)
        {
            nextCall.Invoke();
        }
        
    }
}
