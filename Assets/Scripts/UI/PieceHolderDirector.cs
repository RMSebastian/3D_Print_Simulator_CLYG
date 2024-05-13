using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceHolderDirector : MonoBehaviour
{
    //Debe hablar con StepManager, para verificar que la pieza seleccionada, sea la pieza siguiente
    //se realizara con un ID, 
    private List<PieceHolderManager> pieceHolders = new List<PieceHolderManager>();
    private BuildingStepManager buildingStepManager;
    private void Start()
    {
        buildingStepManager = FindAnyObjectByType<BuildingStepManager>();
    }
    public void AddToListPieceHolder(PieceHolderManager pieceHolderManager)=>pieceHolders.Add(pieceHolderManager);
    public void OnClick(int ID, GameObject go)
    {
        buildingStepManager.CheckPiece(ID,go);
    }
}
