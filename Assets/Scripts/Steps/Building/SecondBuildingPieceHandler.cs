using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// It's in charge of prepare the first parts of a building piece
/// It's composite form BuildingPieceHandler
/// It's hast its manager named SecondBuildingManager to control how these pieces are used
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class SecondBuildingPieceHandler : BuildingPieceHandler
{
    #region Variables
    public Color selectedColor;
    public Vector3 CameraPosition;
    public Quaternion CameraRotation;
    public float cameraOrtographicSize = 0.1f;

    public GameObject signalUI;

    protected bool firstTouch;
    protected bool canInteract = true;

    protected AudioSource source;
    protected SecondBuildingManager secondBuildingManager;
    protected CameraHandler cameraHandler;
    protected List<MeshRenderer> layPieceMeshRenderers = new List<MeshRenderer>();
    #endregion

    #region UnityMethods
    protected override void Start()
    {
        maxTime = 0.2f;
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        base.Start();
        cameraHandler = Camera.main.GetComponent<CameraHandler>();
        if (transform.parent.TryGetComponent<SecondBuildingManager>(out SecondBuildingManager fbph))
        {
            secondBuildingManager = fbph;
            fbph.AddToPiecesList(this);
        }
    }
    #endregion

    #region Override PieceMethods
    public override void PreparePiece(bool showPiece)
    {
        base.PreparePiece(showPiece);
        SetPiece();
    }
    public override void DeactivatePiece()
    {
        base.DeactivatePiece();
        signalUI?.SetActive(false);
    }
    #endregion

    #region PieceMethods
    public void SetCanInteract(bool interact)
    {
        if (firstTouch) return;
        canInteract = interact;
        signalUI?.SetActive(interact);
    }
    protected void SetPiece()
    {
        if (!firstInstantiate)
        {
            defaultMaterial = layPiece.GetComponentInChildren<MeshRenderer>().material;
            if (layPieceMeshRenderer != null) layPieceMeshRenderer.material = glowMaterial;
            else
            {
                layPieceMeshRenderers = new List<MeshRenderer>(layPiece.GetComponentsInChildren<MeshRenderer>());
                foreach (MeshRenderer mr in layPieceMeshRenderers) mr.material = glowMaterial;
            }
            layPiece.gameObject.layer = 6; //Interactable
            layPiece.transform.position = layPosition;
            layPiece.transform.rotation = layRotation;
            embedPiece?.SetActive(true);
            layPiece?.SetActive(true);
            signalUI?.SetActive(true);
        }

    }
    #endregion

    #region MovementMethods
    public override void BeginMovement()
    {
        if (!canInteract) return;
        if (!firstTouch)
        {
            StartCoroutine(InterruptClick());
            firstTouch = true;
            cameraHandler.SetMinigameCameraTransform(CameraPosition, CameraRotation, cameraOrtographicSize);
            cameraHandler.SetMinigameCameraLookAt(this.gameObject);
            cameraHandler.ActivateMinigameCamera();
            secondBuildingManager.OnSelectedOneBuild(this.gameObject);
            signalUI?.SetActive(false);
            return;
        }

    }
    public void Pass()
    {
        base.BeginMovement();
        PlaySound();
    }
    protected IEnumerator InterruptClick()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.5f); //Change when the transition time within camera changes
        canInteract = true;
    }
    protected override void FinishedMovement()
    {
        layPiece.gameObject.layer = 0;

        if (layPieceMeshRenderer != null) layPieceMeshRenderer.material = defaultMaterial;
        else foreach (MeshRenderer mr in layPieceMeshRenderers) mr.material = defaultMaterial;
        cameraHandler.ActivateMainCamera();
        secondBuildingManager.OnSelectedOneBuild();
        base.FinishedMovement();
    }
    public override void BuildingDone()
    {
        signalUI?.SetActive(false);
        base.BuildingDone();
    }
    #endregion
    public void PlaySound()
    {
        source.Play();
    }
}
