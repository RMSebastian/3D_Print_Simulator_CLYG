using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField]private PagesEnum pageTypes;
    [SerializeField]private StepsEnum stepTypes;
    private void Awake()
    {
        ManualManager manualManager = FindObjectOfType<ManualManager>();

        if (manualManager != null) manualManager.AddSheetToManual(this.gameObject, pageTypes,stepTypes);

        this.gameObject.SetActive(false);
    }
}
