using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestManager : MonoBehaviour, IInteractable
{
    [Header("Events")]
    [SerializeField] protected UnityEvent finishedEvents;
    [SerializeField] protected UnityEvent interactEvents;
    [Header("Interactable Object")]
    [SerializeField] protected GameObject interactableGO;
    protected TestingStepManager stepManager;
    [Header("Interactable UI")]
    [SerializeField] protected Button beggingButton;
    protected virtual void Start()
    {
        stepManager = (TestingStepManager)GameManager.Instance.GetManager<TestingStepManager>();
        stepManager.AddTestManager(this);
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
        stepManager.NextTestPiece();
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
