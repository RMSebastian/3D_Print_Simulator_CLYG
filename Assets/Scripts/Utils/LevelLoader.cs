using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    //Hacer esto un singleton con 4 animaciones, 2 de cambio y dos de idle
    public AnimationUnityEvents animEvents;
    private static LevelLoader instance;
    [SerializeField] private float transitionTime;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(this);
        }

    }
    public void Screen(UnityAction action = null)
    {
        StartCoroutine(LoadingLevel(" ", false, action));
    }
    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadingLevel(sceneName,true));
        GameManager.Instance.CloseSimulator();
    }
    private IEnumerator LoadingLevel(string sceneName, bool changeLevel, UnityAction action = null)
    {
        anim.SetTrigger("Start");
        animEvents.ResetOnBeginEvent();
        animEvents.ResetOnEndEvent();
        yield return null;
        float animLenght = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animLenght);
        if(changeLevel)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoad.isDone) yield return null;
        }
        yield return null;
        action?.Invoke();
        anim.SetTrigger("End");
    }
    public static LevelLoader Instance => instance;
}
