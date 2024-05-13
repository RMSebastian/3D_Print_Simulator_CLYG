using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private PieceDescriptionManager pieceDescriptionManager;

    public PieceDescriptionManager PieceDescriptionManager() => pieceDescriptionManager;

}
