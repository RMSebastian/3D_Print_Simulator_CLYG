using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPieceManagerUI : MonoBehaviour
{
    [SerializeField] private List<BuildPieceHandlerUI> pieceHolders = new List<BuildPieceHandlerUI>();
    private BuildingStepManager buildingStepManager;
    private void Start()
    {
        buildingStepManager = FindAnyObjectByType<BuildingStepManager>();
        int i = 0;
        foreach (var piece in pieceHolders)
        {
            piece.SubscribeToAction(OnClick);
            piece.ID = i;
            i++;
        }
    }
    private void OnClick(int ID,GameObject go)
    {
        buildingStepManager.CheckPiece(ID, go);
    }
}
