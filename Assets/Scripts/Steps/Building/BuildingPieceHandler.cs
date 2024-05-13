using System.Collections;
using UnityEngine;
/// <summary>
/// Father-class of FirstBuildingManager and SecondBuildingPieceHandler
/// Here are the main methods for the Building step
/// Prepare, deactivate, start or finished movement of the lay and embedpiece that where prepare on the inspector
/// It has an Interface that make posible the interaccion with the SelectorHandler
/// 
/// For use the children you add to prepare the gameobjects in this way
/// 
/// BuildingPieceHanlderComposite (GO)
///     -> 3D Model (reference this as laypiece and embedpiece on the inspector)
/// Then, use the buttons to create position (layPosition, embedPosition, cameraPosition) from the 3D model transform at the moment
/// </summary>
public abstract class BuildingPieceHandler : MonoBehaviour, IInteractable
{
    #region Variables
    public int ID;

    [SerializeField] protected Material glowMaterial;
    [SerializeField] protected Material defaultMaterial;

    public GameObject layPiece;
    public GameObject embedPiece;

    protected Collider layPieceCollider;
    protected Collider embedPieceCollider;

    protected MeshRenderer layPieceMeshRenderer;
    protected MeshRenderer embedPieceMeshRenderer;

    protected bool firstInstantiate;
    protected bool doingMovement;
    protected bool doneOffset;
    protected bool doneBuildingThisPiece;
    
    protected float maxTime = 0.25f;

    protected Vector3 offset;

    #endregion

    #region PieceColocation
    protected virtual void Start()
    {
        InitialConfiguration();
    }
    /// <summary>
    /// Set the initial configurations
    /// </summary>
    protected virtual void InitialConfiguration()
    {
        DeactivatePiece();
        layPieceCollider = layPiece.GetComponent<Collider>();
        embedPieceCollider = embedPiece.GetComponent<Collider>();
        layPieceMeshRenderer = layPiece.GetComponent<MeshRenderer>();
        embedPieceMeshRenderer = embedPiece.GetComponent<MeshRenderer>();
    }    
    /// <summary>
    /// Deactivated GO embedpiece and laypiece
    /// </summary>
    public virtual void DeactivatePiece()
    {
        embedPiece?.SetActive(false);
        layPiece?.SetActive(false);
    }
    /// <summary>
    /// Activated GO embedpiece and laypiece
    /// </summary>
    public virtual void ActivatePiece()
    {
        embedPiece?.SetActive(true);
        layPiece?.SetActive(true);
    }
    /// <summary>
    /// Makes the necessary preparations to start the selection of piecess
    /// </summary>
    /// <param name="showPiece">Show the embedPiece on true</param>
    public virtual void PreparePiece(bool showPiece)
    {
        ActivatePiece();
        if(!doneOffset)SetOffsetPiece();
    }
    /// <summary>
    /// To do animations correctly, an offset its needed to position the saved Vector3 correctly.
    /// </summary>
    public virtual void SetOffsetPiece()
    {
        doneOffset = true;
        offset = transform.position - Vector3.zero;
        layPosition = layPosition + offset;
        embedPosition = embedPosition + offset;
    }
    #endregion

    #region PieceMovement
    /// <summary>
    /// Methods from the interface
    /// </summary>
    public void Interact()
    {
        BeginMovement();
    }
    /// <summary>
    /// Initial state of the animation, it verified before doing the animation
    /// </summary>
    public virtual void BeginMovement()
    {
        if(!doingMovement) 
        {
            doingMovement = true;
            StartCoroutine(StartMovement());
        }
    }
    /// <summary>
    /// Coroutine that lerps the laypiece to the embedpiece position, ones finished, calls the method FinishedMovement
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator StartMovement()
    {
        float time = 0;
        while(time < maxTime)
        {
            layPiece.transform.position = Vector3.Lerp(layPosition,embedPosition,time/maxTime);
            layPiece.transform.rotation = Quaternion.Lerp(layRotation,embedRotation,time/maxTime);
            time = (time + Time.deltaTime);
            yield return null;
        }
        yield return null;
        FinishedMovement();
    }
    /// <summary>
    /// Set final settings after finishing the StartMovementCoroutine
    /// </summary>
    protected virtual void FinishedMovement()
    {
        SetTransform(layPiece.gameObject.transform, embedPosition, embedRotation);
        BuildingDone();
    }
    public virtual void BuildingDone()
    {
        doneBuildingThisPiece = true;
        if (layPieceCollider != null)
        {
            layPieceCollider.enabled = false;
            layPieceCollider = null;
        }
        if (embedPieceCollider != null)
        {
            embedPieceCollider.enabled = false;
            embedPieceCollider = null;
        }
        if (layPiece != null)
        {
            layPiece = null;
        }
        if (embedPiece != null)
        {
            embedPiece.SetActive(true);
            embedPiece = null;
        }
        embedPieceMeshRenderer = null;
        layPieceMeshRenderer = null;
    }
    #endregion

    #region Transform Method Setter/Getters
    [HideInInspector] public Vector3 layPosition = new Vector3();
    [HideInInspector] public Quaternion layRotation = new Quaternion();
    [HideInInspector] public Vector3 embedPosition = new Vector3();
    [HideInInspector] public Quaternion embedRotation = new Quaternion();
    /// <summary>
    /// Set a gameobject position in the world position create before
    /// </summary>
    /// <param name="transform">transform to move</param>
    /// <param name="position">position to move the piece</param>
    /// <param name="quaternion">rotation to move the piece</param>
    public void SetTransform(Transform transform, Vector3 position, Quaternion quaternion)
    {
        transform.position = position;
        transform.rotation = quaternion;
    }
    /// <summary>
    /// Create a position based on the actual gameobject position
    /// </summary>
    /// <param name="transform">transform based</param>
    /// <param name="position">position based</param>
    /// <param name="quaternion">rotation based</param>
    public void CreateTransform(Transform transform, ref Vector3 position, ref Quaternion quaternion)
    {
        position = transform.position;
        quaternion = transform.rotation;
    }
    /// <summary>
    /// Reset the position data saved
    /// </summary>
    /// <param name="position">reset this position</param>
    /// <param name="quaternion">reset this rotation</param>
    public void ResetTransform(ref Vector3 position, ref Quaternion quaternion)
    {
        position = new Vector3();
        quaternion = new Quaternion();
    }

    #endregion
}
