using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// One of the main Manager, this is in charge of how the world on Preparing step works
/// It's receive calls from PreparePieceManager
/// When the whole preparing step its done, calls to GameManager to pass to the next step
/// </summary>
public class PreparingStepManager : StepManager
{
    #region Variables
    private List<MinigameManager> minigameManagers = new List<MinigameManager>();
    private MinigameManager currentmm;
    private int mmIndex = 0;
    #endregion

    #region AddToListMethods
    public void AddMinigameManager(MinigameManager minigameManager)
    {
        this.minigameManagers.Add(minigameManager);
    }
    private void SortLists()
    {
        minigameManagers.Sort((left, right) => left.name.CompareTo(right.name));
    }
    #endregion
    public override void StartStep()
    {
        base.StartStep();
        SortLists();
        ActivateStep();
        progressBarManager.NextSlider();
        int value = minigameManagers.Count;
        progressBarManager.Init(ID, value);
    }
    public void ActivateStep()
    {
        currentmm = minigameManagers[mmIndex];
        currentmm.InitiateInteraction();
    }
    public void NextPreparePiece()
    {
        mmIndex++;
        OnDevelopmentStep?.Invoke();
        if(mmIndex > minigameManagers.Count-1)
        {
            FinishedStep();
        }
        else
        {
            currentmm = minigameManagers[mmIndex];
            currentmm.InitiateInteraction();
        }
    }
    public override void DeleteData()
    {
        foreach (MinigameManager mm in minigameManagers) Destroy(mm);
        minigameManagers = null;
        currentmm = null;
    }
}
