using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SBMWaitableMinigame : WaitableBase
{
    [Header("UI Components")]
    [SerializeField] private WaitableBase badAnimation;
    [SerializeField] private WaitableBase goodAnimation;
    [SerializeField] private GameObject pnlBarMenu;
    [SerializeField] private Slider sldBar;
    [SerializeField] private Button button;
    [Header("Configuration")]
    [SerializeField] protected float interceptDistance = 50f;
    [SerializeField] protected float amplitudeMultiplier = 50f;
    [SerializeField] protected float frequencyMultiplier = 2.5f;

    private bool startCounting = false;
    private int minWinValue = 33;
    private int maxWinValue = 67;
    private float cosTimer = 0;
    private UnityAction request;

    public override void HandleRequest(UnityAction finalRequest = null)
    {
        request = finalRequest;
        InitialConfiguration();
    }
    private void Update()
    {
        if (!startCounting) return;
        cosTimer += Time.deltaTime;
        sldBar.value = amplitudeMultiplier * Mathf.Cos(cosTimer* frequencyMultiplier) + interceptDistance;
    }
    private void InitialConfiguration()
    {
        cosTimer = 0;
        button.gameObject.SetActive(true);
        sldBar.gameObject.SetActive(true);
        startCounting = true;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(CanPass);
    }
    public void CanPass()
    {
        startCounting = false;
        if (sldBar.value >= minWinValue && sldBar.value <= maxWinValue)
        {
            goodAnimation.HandleRequest(Pass);
            cosTimer = 0;
            button.gameObject.SetActive(false);
            sldBar.gameObject.SetActive(false);
            startCounting = false;
        }
        else
        {
            badAnimation.HandleRequest(SetTimer);
        }
    }
    private void Pass()
    {

        base.HandleRequest(request);
    }
    private void SetTimer()
    {
        int rnd = Random.Range(0, 2);
        cosTimer = (rnd == 0) ? 0 : 1.57f;
        startCounting = true;
    }
}
