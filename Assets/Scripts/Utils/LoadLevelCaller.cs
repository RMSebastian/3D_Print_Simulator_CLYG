using UnityEngine;
public class LoadLevelCaller : MonoBehaviour
{
    public void ChangeLevel(string sceneName)
    {
        LevelLoader.Instance.LoadLevel(sceneName);
    }
}
