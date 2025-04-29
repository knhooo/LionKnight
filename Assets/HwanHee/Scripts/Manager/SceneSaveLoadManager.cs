using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaveLoadManager : MonoBehaviour
{
    public static SceneSaveLoadManager instance;

    public bool canDataSave = true;

    [SerializeField] private SceneFader sceneFader;
    private string sceneName;

    private void Awake()
    {
        #region 싱글톤
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        #endregion

        sceneFader = Instantiate(sceneFader).GetComponent<SceneFader>();
        sceneFader.transform.SetParent(transform);
    }

    public void StartLoadScene(string _sceneName)
    {
        sceneName = _sceneName;
        DataManager.instance.SaveData();
        sceneFader.FadeToScene();
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
