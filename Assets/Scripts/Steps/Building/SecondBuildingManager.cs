using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Manager of SecondBuildignPieceHandlers where are children of it
/// This control how they act,
///     the camera position,
///     the time on press, 
///     the Minigame UI 
///     and the messages to BuildingStepManager when finished all pieces
/// </summary>
public class SecondBuildingManager : MonoBehaviour
{
    #region Variables
    public int ID;
    
    public List<SecondBuildingPieceHandler> secondPieces = new List<SecondBuildingPieceHandler>();
    
    [SerializeField] private BuildingStepManager stepManager;
    
    [SerializeField] private WaitableBase minigame;
    
    private int amount;
    #endregion

    #region UnityMethods
    private void Start()
    {
        if(stepManager == null) 
            stepManager = (BuildingStepManager)GameManager.Instance.GetManager<BuildingStepManager>();
        stepManager.AddToList(this);
    }
    public void Init()
    {
        foreach (BuildingPieceHandler piece in secondPieces) piece.PreparePiece(true);
    }
    #endregion

    #region OnSelectedObjectMethods
    public void OnSelectedOneBuild(GameObject go = null)
    {
        bool goIsNotNull = (go != null) ? true : false;

        foreach (SecondBuildingPieceHandler piece in secondPieces)
        {
            if(!goIsNotNull) piece.SetCanInteract(true);
            else if(goIsNotNull && piece.gameObject == go)
            {
                minigame.HandleRequest(piece.Pass);
                piece.SetCanInteract(true);
            }
            else piece.SetCanInteract(false);
        }

        if (!goIsNotNull)
        {
            amount -= 1;
            stepManager.BuildingTwoProgress();
        }
        if(amount <= 0) stepManager.BuildingTwoDone();
    }
    #endregion

    #region Generation List With SecondBuildingsHandlers
    public void AddToPiecesList(SecondBuildingPieceHandler sbph)
    {
        secondPieces.Add(sbph);
        amount++;
    }
    #endregion
    public void DeleteData()
    {
        foreach (SecondBuildingPieceHandler sbph in secondPieces) Destroy(sbph);
        secondPieces = null;
    }
}
