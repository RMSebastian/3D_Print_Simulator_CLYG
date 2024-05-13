using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Start Configurations")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Button btnSFX;
    [SerializeField] private Button btnMusic;

    [Header("Interaction Configurations (PauseMenu)")]
    [SerializeField] private RectTransform pnlBackground;
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private Button btnBlocker;
    [SerializeField] private Button btnPause;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite resumeSprite;
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private float closeHeight = 60;
    [SerializeField] private float openHeight = 280;
    
    
    private bool isOpen;
    private bool hasMusic;
    private bool hasSFX;
    private void Start()
    {
        float sound;
        if(PlayerPrefs.HasKey("SFX"))
        {
            sound = PlayerPrefs.GetFloat("SFX");
            btnSFX.targetGraphic.color = (sound > -10) ?btnSFX.colors.normalColor: btnSFX.colors.pressedColor;
            hasSFX = (sound > -10) ? true : false;
            audioMixer.SetFloat("SFX", sound);
        }
        else
        {
            btnSFX.targetGraphic.color = btnSFX.colors.normalColor;
            audioMixer.SetFloat("SFX", 0);
            PlayerPrefs.SetFloat("SFX", 0);
            hasSFX = true;
        }
        sound = PlayerPrefs.GetFloat("Music");
        if (PlayerPrefs.HasKey("Music"))
        {
            sound = PlayerPrefs.GetFloat("Music");
            btnMusic.targetGraphic.color = (sound > -10) ? btnMusic.colors.normalColor : btnMusic.colors.pressedColor;
            hasMusic = (sound > -10) ? true : false;
            audioMixer.SetFloat("Music", sound);
        }
        else
        {
            btnMusic.targetGraphic.color = btnSFX.colors.normalColor;
            audioMixer.SetFloat("Music", 0);
            PlayerPrefs.SetFloat("Music", 0);
            hasMusic = true;
        }
        PlayerPrefs.Save();
    }
    public void OnClickButton()
    {
        StopAllCoroutines();
        
        isOpen = !isOpen;
        if (isOpen)
        {
            musicManager.Pause();
            StartCoroutine(Lerp(closeHeight, openHeight, finalAction: () => ButtonActivation(isOpen)));
            btnPause.image.sprite = resumeSprite;
            Time.timeScale = 0;
        }
        else
        {
            musicManager.Play();
            StartCoroutine(Lerp(openHeight, closeHeight,initialAction: () => ButtonActivation(isOpen)));
            btnPause.image.sprite = pauseSprite;
            Time.timeScale = 1;
        }
        btnBlocker.gameObject.SetActive(isOpen);
    }
    public void ButtonActivation(bool isActive)
    {
        foreach (var button in buttons)
        {
            button.enabled = isActive;
        }
    }
    protected IEnumerator Lerp(float startValue, float endValue,UnityAction initialAction = null, UnityAction finalAction =null)
    {
        initialAction?.Invoke();
        float lerpDuration = 0.25f;
        float valueToLerp;
        float timeElapsed = 0;
        Vector2 currentSize;
        while (timeElapsed < lerpDuration)
        {
            float sinFunc = Mathf.Sin((timeElapsed * Mathf.PI) / (2 * lerpDuration));
            valueToLerp = Mathf.Lerp(startValue, endValue, sinFunc);
            currentSize = pnlBackground.sizeDelta;
            currentSize.y = valueToLerp;
            pnlBackground.sizeDelta = currentSize;
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        currentSize = pnlBackground.sizeDelta;
        currentSize.y = endValue;
        pnlBackground.sizeDelta = currentSize;
        finalAction?.Invoke();
    }

    public void HomeButton()
    {
        Time.timeScale = 1.0f;
        LevelLoader.Instance?.LoadLevel("TitleScene");
    }
    public void SFXButton()
    {
        float sound = (hasSFX)? -80 : 0;
        btnSFX.targetGraphic.color = (sound > -10) ? btnSFX.colors.normalColor : btnSFX.colors.pressedColor;
        audioMixer.SetFloat("SFX", sound);
        PlayerPrefs.SetFloat("SFX", sound);
        hasSFX = !hasSFX;
        PlayerPrefs.Save();
    }
    public void MusicButton()
    {
        float sound = (hasMusic) ? -80 : 0;
        btnMusic.targetGraphic.color = (sound > -10) ? btnMusic.colors.normalColor : btnMusic.colors.pressedColor;
        audioMixer.SetFloat("Music", sound);
        PlayerPrefs.SetFloat("Music", sound);
        hasMusic = !hasMusic;
        PlayerPrefs.Save();
    }
    public void CloseApplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        Debug.Log("QUIT");
#endif
    }
}
