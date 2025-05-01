using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaveLoadManager : Singleton<SceneSaveLoadManager>
{
    public bool canDataSave = true;

    [SerializeField] private SceneFader sceneFader;
    private string sceneName;

    protected override void Awake()
    {
        base.Awake();

        sceneFader = Instantiate(sceneFader).GetComponent<SceneFader>();
        sceneFader.transform.SetParent(transform);
    }

    public void StartLoadScene(string _sceneName)
    {
        sceneName = _sceneName;
        DataManager.instance.SaveData();
        sceneFader.FadeToScene();

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
    }
}
