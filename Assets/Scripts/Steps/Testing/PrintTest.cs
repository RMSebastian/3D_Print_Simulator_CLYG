using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PrintTest : TestHandler
{
    #region Variables
    [SerializeField] protected GameObject UI;
    [SerializeField] protected TextMeshProUGUI preheatMessageGO;
    [SerializeField] protected Button menuButton;
    [SerializeField] protected GameObject SDGO;
    [SerializeField] protected Transform SDEmbedPosition;
    [SerializeField] protected Transform SDLayPosition;
    [SerializeField] protected WaitableBase soundAnimation;
    protected AudioSource source;
    protected WaitForSeconds bfmTime = new WaitForSeconds(0.2f);
    #endregion

    #region Override Methods
    protected override void Start()
    {
        source = GetComponent<AudioSource>();
        base.Start();
        InitialConfiguration();
    }
    protected void InitialConfiguration()
    {
        SDGO.SetActive(false);
    }
    public override void PrepareMinigame()
    {
        SDGO.SetActive(true);
        UI.gameObject.SetActive(true);
        preheatMessageGO.gameObject.SetActive(false);
        maxTime = 0.25f;
        source.Play();
        StartCoroutine(MoveAnimation(SDGO, SDEmbedPosition.position, () =>
        {
            SetCameraPosition(0);
            maxTime = 0.95f;
        }));
        base.PrepareMinigame();
    }
    public override void FinishedMinigame()
    {
        SetCameraPosition(-1);
        preheatMessageGO.gameObject.SetActive(false);
        base.FinishedMinigame();
    }

    #endregion

    #region OnClick Methods
    public void PrintComplete()
    {
        menuButton.interactable = false;
        preheatMessageGO.gameObject.SetActive(true);
        StartCoroutine(BeforeFinishingMinigame());
    }
    #endregion

    #region Proper Animations
    protected IEnumerator BeforeFinishingMinigame()
    {
        soundAnimation.HandleRequest();
        float time = 0;
        bool alpha = false;
        preheatMessageGO.text = "Printing...";
        while (time < 2)
        {
            preheatMessageGO.color = (alpha) ? Color.white : Color.clear;
            alpha = !alpha;
            time += 0.2f;
            yield return bfmTime;
        }
        yield return null;
        FinishedMinigame();
    }
    #endregion
}
