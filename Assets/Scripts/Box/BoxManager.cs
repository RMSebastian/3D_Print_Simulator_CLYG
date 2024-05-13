using UnityEngine;

public class BoxManager : MonoBehaviour, IInteractable
{
    [SerializeField] private BoxInteractionHandler firstInteractor;
    private int index = -1;
    public GameObject box;
    private void Awake()
    {
        if(LevelLoader.Instance != null)
        {
            LevelLoader.Instance.animEvents.AddOnEndEvent(Interact);
        }
        else
        {
            Interact();
        }
        box?.SetActive(true);
    }
    /// <summary>
    /// LoadAsyncronic tratara de activarte, o el gamemanager, una vez el fundido en blanco termine
    /// </summary>
    [ContextMenu("Interact")]
    public void Interact()
    {
        firstInteractor?.HandlerRequest(index,FinishedBoxInteraction);
        index++;
    }
    private void FinishedBoxInteraction()
    {
        GameManager.Instance?.StartSimulator();
        this.gameObject.SetActive(false);
    }
    
}
