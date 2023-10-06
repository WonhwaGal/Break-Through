using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    void LoadNextScene();
    void OnLoadEvent(Scene scene, LoadSceneMode mode);
}
