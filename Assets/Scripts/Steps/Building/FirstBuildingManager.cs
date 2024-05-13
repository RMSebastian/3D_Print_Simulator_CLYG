using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// It's in charge of prepare the first parts of a building piece
/// It's composite form BuildingPieceHandler
/// It's send messages to BuildingStepManager on finished
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class FirstBuildingManager : BuildingPieceHandler
{
    #region Variables
    [SerializeField] private PositionsLibrary positionsLibrary;
    [SerializeField] private Transform minigamePosition;
    [SerializeField] private BuildingStepManager stepManager;
    [SerializeField] private FBMWaitableMinigame minigame;
    [SerializeField] private AudioSource source;
    private List<MeshRenderer> embedPieceMeshRenderers = new List<MeshRenderer>();
    private List<MeshRenderer> layPieceMeshRenderers = new List<MeshRenderer>();
    private List<Material> defaultMaterialsRenderers = new List<Material>();
    #endregion
    protected override void Start()
    {
        source = GetComponent<AudioSource>();
        base.Start();
    }
    #region Override PieceMethods
    protected override void InitialConfiguration()
    {
        base.InitialConfiguration();
        if(stepManager == null) stepManager = (BuildingStepManager)GameManager.Instance.GetManager<BuildingStepManager>();
        stepManager.AddToList(this);
    }
    public override void PreparePiece(bool showPiece)
    {
        base.PreparePiece(showPiece);   
        CreateBothPieces();
        SetMaterialOnMesh(true, defaultMaterial, defaultMaterialsRenderers, layPieceMeshRenderer,layPieceMeshRenderers);
        SetMaterialOnMesh(showPiece, glowMaterial,null,embedPieceMeshRenderer,embedPieceMeshRenderers);
        CanUsePiece(showPiece);
    }
    #endregion

    #region PieceMethods
    protected void CanUsePiece(bool use)
    {
        layPiece.SetActive(true);
        embedPiece.SetActive(use);
        embedPiece.GetComponent<BoxCollider>().enabled = use;
    }
    protected void CreateBothPieces()
    {
        if (!firstInstantiate)
        {
            firstInstantiate = true;

            layPiece = Instantiate(embedPiece, layPosition, layRotation, transform);
            layPiece.gameObject.layer = 0; //Default
            
            if (layPieceMeshRenderer == null) layPieceMeshRenderers = new List<MeshRenderer>(layPiece.GetComponentsInChildren<MeshRenderer>());
            
            if(embedPieceMeshRenderer != null) defaultMaterial = embedPieceMeshRenderer.material;
            else
            {
                embedPieceMeshRenderers = new List<MeshRenderer>(embedPiece.GetComponentsInChildren<MeshRenderer>());
                int i = 0;
                foreach(MeshRenderer mr in embedPieceMeshRenderers)
                {
                    defaultMaterialsRenderers.Add(embedPieceMeshRenderers[i].material);
                    i++;
                }
            }

            embedPiece.gameObject.layer = 6; //Interactable
            embedPiece.transform.position = embedPosition;
            embedPiece.transform.rotation = embedRotation;
            embedPiece?.SetActive(true);
            layPiece?.SetActive(true);
        }
    }
    protected void SetMaterialOnMesh(bool canShow, Material material = null, List<Material> materialList = null, MeshRenderer mesh = null, List<MeshRenderer> meshes = null)
    {
        if (mesh != null)
        {
            mesh.material = material;
            mesh.enabled = canShow;
        }
        else if(meshes != null)
        {
            bool useList = (materialList != null && materialList[0] != null) ?true:false;
            int i = 0;
            foreach (MeshRenderer mr in meshes)
            {
                mr.material = (useList) ? materialList[i] : material;
                mr.enabled = canShow;
                i++;
            }
        }
    }
    #endregion
    #region Override MovementMethods
    protected override IEnumerator StartMovement()
    {
        if (minigame == null)
        {
           yield return base.StartMovement();
        }
        else
        {
            float time = 0;
            while (time < maxTime)
            {
                layPiece.transform.position = Vector3.Lerp(layPosition, minigamePosition.position, time/maxTime);
                layPiece.transform.rotation = Quaternion.Lerp(layRotation, minigamePosition.rotation, time/maxTime);
                time = (time + Time.deltaTime);
                yield return null;
            }
            yield return null;
            SetTransform(layPiece.gameObject.transform, minigamePosition.position, minigamePosition.rotation);
            minigame.BeginMinigame(layPiece, positionsLibrary, embedRotation);
            minigame.HandleRequest(FinishedMovement);
        }


    }
    protected override void FinishedMovement()
    {
        PlaySound();
        embedPiece.gameObject.layer = 0; //Default
        int i = 0;
        if(embedPieceMeshRenderer != null)
        {
            embedPieceMeshRenderer.material = defaultMaterial;  
        }
        else
        {
            foreach (MeshRenderer mr in embedPieceMeshRenderers)
            {
                mr.material = defaultMaterialsRenderers[i];
                mr.enabled = true;
                i++;
            }
        }
        Destroy(layPiece);
        base.FinishedMovement();
        stepManager.BuildingOneDone();
    }
    public void PlaySound()
    {
        source.Play();
    }
    #endregion
}
