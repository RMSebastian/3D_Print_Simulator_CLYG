using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManualManager : MonoBehaviour
{
    #region Private Variables
    [Header("GUI Configuration")]
    [SerializeField] private GameObject pnlGuideMenu;
    [SerializeField] private GameObject pnlInventoryMenu;

    
    private List<GameObject> pnlInventoryPages = new List<GameObject>();
    private Dictionary<StepsEnum,List<GameObject>> pnlGuidePagesDict = new Dictionary<StepsEnum, List<GameObject>>();

    private StepsEnum currentStep = StepsEnum.Building;
    private PagesEnum currentPage = PagesEnum.Guide;

    private int guideSheetIndex = 0;
    private int inventorySheetIndex = 0;
    #endregion
    #region Unity Methods
    private void Awake()
    {
        pnlGuideMenu.SetActive(true);
        pnlInventoryMenu.SetActive(true);
    }
    private void Start()
    {
        InitialConfiguration();
    }
    #endregion
    #region PublicMethods
    /// <summary>
    /// Call from PageManager.cs to add itself to a list depending of its characteristics
    /// This define the amount of pages for Inventory/Guide to pass on
    /// </summary>
    /// <param name="page"> The object with all the UI</param>
    /// <param name="pageType"> Its for the Guide part or Inventory part</param>
    /// <param name="step">If it is for Guide, for what step</param>
    public void AddSheetToManual(GameObject page, PagesEnum pageType,StepsEnum step)
    {
        switch (pageType)
        {
            case PagesEnum.Guide:
                if (pnlGuidePagesDict.ContainsKey(step))
                {
                    pnlGuidePagesDict[step].Add(page);
                }
                else
                {
                    List<GameObject> newList = new List<GameObject> {page};
                    pnlGuidePagesDict.Add(step, newList);
                    
                }
                pnlGuidePagesDict[step].Sort((left, right) => left.name.CompareTo(right.name));
                break;
            case PagesEnum.Inventory:
                pnlInventoryPages.Add(page);
                pnlInventoryPages.Sort((left, right) => left.name.CompareTo(right.name));
                break;
            default:
                Debug.LogError("ManualManager AddSheetToManual It's not working correctly");
                break;
        }
    }
    /// <summary>
    /// Call from Guide/Invetory Buttons on ManualMenu UI on GameplayScene
    /// This change the interface withing this two
    /// </summary>
    /// <param name="type"> value of the Enum PagesEnum </param>
    public void ChangePage(int type)
    {
        PagesEnum pageType = (PagesEnum)type;
        currentPage = pageType;
        switch (currentPage)
        {
            case PagesEnum.Guide:
                pnlGuideMenu.SetActive(true);
                pnlInventoryMenu.SetActive(false);
                break;
            case PagesEnum.Inventory:
                pnlGuideMenu.SetActive(false);
                pnlInventoryMenu.SetActive(true);
                break;
            default:
                Debug.LogError("ManualManager ChangePage It's not working correctly");
                break;
        }
    }
    /// <summary>
    /// Call from Left/Right Buttons on ManualMenu UI on GameplayScene
    /// Pass the sheet to left or right depending on the argument receive
    /// </summary>
    /// <param name="page"> 1 moves to the right, -1 moves to the left</param>
    public void ChangeSheet(int page)
    {
        switch (currentPage)
        {
            case PagesEnum.Guide:
                CheckGuideSheet(page);
                break;
            case PagesEnum.Inventory:
                CheckInventorySheet(page);
                break;
            default:
                Debug.LogError("ManualManager ChangeSheet It's not working correctly");
                break;
        }
    }
    /// <summary>
    /// Call from BUilding/Preparing/Testing Buttons on ManualMenu UI on GameplayScene
    /// This Change the key on the dictionary to read new pages
    /// </summary>
    /// <param name="step"> value of the Enum StepEnum</param>
    public void ChangeStep(int step)
    {
        StepsEnum stepType = (StepsEnum)step;
        List<GameObject> currentSheets = pnlGuidePagesDict[currentStep];
        currentSheets[guideSheetIndex].SetActive(false);
        currentStep = stepType;
        guideSheetIndex = 0;
        currentSheets = pnlGuidePagesDict[currentStep];
        currentSheets[guideSheetIndex].SetActive(true);
    }
    #endregion
    #region PrivateMethods
    /// <summary>
    /// This activate the first panels on ManualMenu UI on GameplayScene
    /// </summary>
    private void InitialConfiguration()
    {
        pnlGuideMenu.SetActive(true);
        pnlInventoryMenu.SetActive(false);
        List<GameObject> currentSheets = pnlGuidePagesDict[currentStep];
        currentSheets[0].SetActive(true);
        pnlInventoryPages[0].SetActive(true);
    }
    /// <summary>
    /// Verified if it is posible to change to one sheet to another for Guide Pages
    /// </summary>
    /// <param name="page"> 1 moves to the right, -1 moves to the left</param>
    private void CheckGuideSheet(int page)
    {
        List<GameObject> currentSheets = pnlGuidePagesDict[currentStep];
        int backUp = guideSheetIndex + page;

        if (backUp > currentSheets.Count - 1 || backUp < 0) return;

        currentSheets[guideSheetIndex].SetActive(false);
        currentSheets[backUp].SetActive(true);
        guideSheetIndex = backUp;
    }
    /// <summary>
    /// Verified if it is posible to change to one sheet to another for Inventory Pages
    /// </summary>
    /// <param name="page"> 1 moves to the right, -1 moves to the left</param>
    private void CheckInventorySheet(int page)
    {
        int backUp = inventorySheetIndex + page;
        if (backUp > pnlInventoryPages.Count - 1 || backUp < 0) return;

        pnlInventoryPages[inventorySheetIndex].SetActive(false);
        pnlInventoryPages[backUp].SetActive(true);
        inventorySheetIndex = backUp;


    }
    #endregion
}
