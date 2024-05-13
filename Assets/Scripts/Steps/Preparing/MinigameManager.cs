using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour, IInteractable
{
    [Header("Events")]
    [SerializeField] protected UnityEvent finishedEvents;
    [SerializeField] protected UnityEvent interactEvents;
    [Header("Interactable Object")]
    [SerializeField]protected GameObject interactableGO;
    [Header("Interactable UI")]
    [SerializeField] protected Button beggingButton;
    protected PreparingStepManager stepManager;
    
    protected virtual void Start()
    {
        stepManager = (PreparingStepManager)GameManager.Instance.GetManager<PreparingStepManager>();
        stepManager.AddMinigameManager(this);
        interactableGO.SetActive(false);
        beggingButton.interactable = false;
    }

    protected virtual void Update()
    {
        
    }
    public virtual void PrepareMinigame()
    {

    }
    public virtual void FinishedMinigame()
    {
        finishedEvents?.Invoke();
        stepManager.NextPreparePiece();
    }

    public virtual void InitiateInteraction()
    {
        interactableGO.SetActive(true);
        beggingButton.interactable = true;
    }    
    public void Interact()
    {
        if (!interactableGO.activeSelf) return;
        interactEvents?.Invoke();
        interactableGO.SetActive(false);
        PrepareMinigame();
    }
}
