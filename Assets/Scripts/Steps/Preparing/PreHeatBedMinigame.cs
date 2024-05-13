using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
[RequireComponent(typeof(PlayerInput))]
public class PreHeatBedMinigame : MinigameHandler
{
    #region Variables
    [SerializeField] protected GameObject UI;
    [SerializeField] protected TextMeshProUGUI preheatMessageGO;
    [SerializeField] protected TextMeshProUGUI txtCurrentHETemp;
    [SerializeField] protected TextMeshProUGUI txtHETemp;
    [SerializeField] protected TextMeshProUGUI txtCurrentBTemp;
    [SerializeField] protected TextMeshProUGUI txtBTemp;
    [SerializeField] protected Button menuButton;
    [SerializeField] protected WaitableBase soundAnimation;

    protected WaitForSeconds bfmTime = new WaitForSeconds(0.2f);
    #endregion

    #region Override Methods
    public override void PrepareMinigame()
    {
        SetCameraPosition(0);
        preheatMessageGO.gameObject.SetActive(false);
        UI.gameObject.SetActive(true);
        base.PrepareMinigame();
    }
    public override void FinishedMinigame()
    {
        SetCameraPosition(-1);
        preheatMessageGO.gameObject.SetActive(false);
        UI.gameObject.SetActive(false);
        base.FinishedMinigame();
    }

    #endregion

    #region OnClick Methods
    public void PreheatComplete()
    {
        menuButton.interactable = false;
        preheatMessageGO.gameObject.SetActive(true);
        txtHETemp.text = "200°";
        txtBTemp.text = "60°";
        StartCoroutine(PreHeatAnimation(txtCurrentHETemp,200,3,delegate {StartCoroutine(BeforeFinishingMinigame());}));
        StartCoroutine(PreHeatAnimation(txtCurrentBTemp,60,2));

    }
    #endregion

    #region Proper Animations
    protected IEnumerator BeforeFinishingMinigame()
    {
        soundAnimation.HandleRequest();
        float time = 0;
        bool alpha = false;
        preheatMessageGO.text = "Preheat Complete";
        while (time < 2)
        {
            preheatMessageGO.color = (alpha) ?Color.white:Color.clear;
            alpha = !alpha;
            time += 0.2f;
            yield return bfmTime;
        }
        yield return null;
        FinishedMinigame();
    }
    protected IEnumerator PreHeatAnimation(TextMeshProUGUI go, int finalTemperature, float maxTime, UnityAction action = null)
    {
        float time = 0;
        float i = 0;
        while (time < maxTime)
        {
            go.text = ($"{Mathf.CeilToInt(Mathf.Lerp(i, finalTemperature, time/maxTime))}°");
            time = (time + Time.deltaTime);
            yield return null;
        }
        go.text = ($"{finalTemperature}°");
        yield return null;
        if(action != null)
        {
            action.Invoke();
        }
    }
    #endregion
}
