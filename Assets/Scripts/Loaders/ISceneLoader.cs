using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    void LoadNextScene(bool startFromScratch);
    void OnLoadEvent(Scene scene, LoadSceneMode mode);
}
