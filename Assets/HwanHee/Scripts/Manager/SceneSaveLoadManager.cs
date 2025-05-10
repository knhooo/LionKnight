using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaveLoadManager : Singleton<SceneSaveLoadManager>
{
    public bool canDataSave = true;

    [SerializeField] private SceneFaderCanvas sceneFaderCanvas;
    private string sceneName;

    protected override void Awake()
    {
        base.Awake();

        sceneFaderCanvas.transform.SetParent(transform);
    }

    public void StartLoadScene(string _sceneName, bool isNewGame = false)
    {
        sceneName = _sceneName;

        if (isNewGame)
            DataManager.instance.CreateFile();
        DataManager.instance.SaveData();

        sceneFaderCanvas.FadeToScene();

        BGMManager.instance.BGMFadeOut();
    }

    public void LoadScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DataManager.instance.LoadData();

        BGMManager.instance.ChangeBGM(0.5f);
    }
}
