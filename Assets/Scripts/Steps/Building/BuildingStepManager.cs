using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// One of the main Manager, this is in charge of how the world on Building step works
/// It's receive calls from FirstBuildingManagers and SecondBUildingManagers
/// Also disable UI, like the buttons to prevent the use of buttons tha where already used
/// When the whole building step its doneBuildingThisPiece, calls to GameManager to pass to the next step
/// </summary>
public class BuildingStepManager : StepManager
{
    #region Variables
    [SerializeField] private UnityEvent OnSBMBeginEvent = new UnityEvent();
    [SerializeField] private UnityEvent OnSBMEndEvent = new UnityEvent();
    
    private List<FirstBuildingManager> FBMList = new List<FirstBuildingManager>();
    private List<SecondBuildingManager> SBMList = new List<SecondBuildingManager>();
    private FirstBuildingManager _currentFBM;
    private SecondBuildingManager _currentSBM;
    private int _correctIndexFBM = 0;
    private int _correctIndexSBM = 0;
    private Action actions;
    #endregion
    #region AddToListMethods
    public void AddToList<T>(T data)
    {
        switch (typeof(T))
        {
            case Type t when t == typeof(FirstBuildingManager):
                this.FBMList.Add((FirstBuildingManager)(object)data);
                break;
            case Type t when t == typeof(SecondBuildingManager):
                this.SBMList.Add((SecondBuildingManager)(object)data);
                break;
            default:
                
                break;
        }
        
    }
    private void SortLists()
    {
        FBMList.Sort((left, right) => left.ID.CompareTo(right.ID));
        SBMList.Sort((left, right) => left.ID.CompareTo(right.ID));
    }
    #endregion
    public override void StartStep()
    {
        base.StartStep();
        SortLists();
        progressBarManager.NextSlider();
        int value = FBMList.Count + SBMList.Count;
        progressBarManager.Init(ID,value);
    }
    #region CheckPieceProcessMethods
    public void CheckPiece(int ID, GameObject go)
    {
        _currentFBM?.DeactivatePiece();
        _currentFBM = FBMList.Find(x => x.ID == ID);
        _currentSBM = SBMList.Find(y => y.ID == ID);
        FBMList[_currentFBM.ID].PreparePiece((_currentFBM.ID == _correctIndexFBM)? true :false);
        actions = () => { go.gameObject.SetActive(false); };
    }
    public void BuildingOneDone()
    {
        OnDevelopmentStep?.Invoke();
        if (_currentSBM != null && _currentSBM.ID == _currentFBM.ID)
        {
            if (OnSBMBeginEvent != null) OnSBMBeginEvent.Invoke();
            SBMList[_correctIndexSBM].Init();
        }
        else ResetThings();
        _correctIndexFBM++;   
    }
    public void BuildingTwoProgress() { }//OnDevelopmentStep.Invoke();
    public void BuildingTwoDone()
    {
        OnDevelopmentStep?.Invoke();
        ResetThings();
        _correctIndexSBM++;
    }
    private void ResetThings()
    {
        _currentFBM = null;
        _currentSBM = null; 
        actions?.Invoke();
        if (_correctIndexFBM >= FBMList.Count - 1)
        {
            FinishedStep();
        }
        else if (OnSBMEndEvent != null) OnSBMEndEvent.Invoke();
    }
    public override void DeleteData()
    {
        foreach (FirstBuildingManager fbm in FBMList) Destroy(fbm);
        foreach (SecondBuildingManager sbm in SBMList)
        {
            sbm.DeleteData();
            Destroy(sbm);
        }
        FBMList = null;
        SBMList = null;
        _currentFBM = null;
        _currentSBM = null;
    }
    #endregion
}
