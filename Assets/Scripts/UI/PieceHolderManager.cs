using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceHolderManager : MonoBehaviour
{
    [Header("GUI Information")]
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private Image imgSprite;
    [Header("Piece Information")]
    [SerializeField] private PieceDescription piece;
    private RectTransform rect;

    private PieceDescriptionManager pieceDescriptionManager;
    private PieceHolderDirector pieceHolderDirector;
    private void Start()
    {
        InitialConfiguration();
        pieceDescriptionManager = FindAnyObjectByType<InventoryManager>().PieceDescriptionManager();
        rect = GetComponent<RectTransform>();
        pieceHolderDirector = transform.parent.GetComponent<PieceHolderDirector>();
        pieceHolderDirector.AddToListPieceHolder(this);
    }
    /// <summary>
    /// With the class PieceDescription define in the Inspector
    /// We define the container with information of pieces
    /// </summary>
    private void InitialConfiguration()
    {
        txtName.text = piece.Name.ToUpper();
        imgSprite.sprite = piece.Image;
    }
    /// <summary>
    /// Done by a button on the PieceHolder UI
    /// It´s sends the list of strings to prepare de descriptions
    /// </summary>
    public void OnPressImage()
    {
        if (pieceDescriptionManager != null)
        {
            pieceDescriptionManager.gameObject.SetActive(true);

            pieceDescriptionManager.SetListOfDescription(piece.Parts);

            pieceDescriptionManager.SetButtonAction(delegate { pieceHolderDirector.OnClick(piece.ID, this.gameObject); }) ;
            Vector3 position = rect.position;
            pieceDescriptionManager.SetPosition(position);
        }
    }

}
