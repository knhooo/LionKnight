using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Splines.ExtrusionShapes;


public class optionManage : MonoBehaviour
{
    public GameObject main;
    public GameObject load;
    public GameObject complete;
    public GameObject setting;
    public string sceneName;

    public SaveSlotUI[] saveSlotUIs;
    public CanvasGroup saveSelect;

    [Header("Credit")]
    public VideoPlayer credit;
    public GameObject creditUI;
    public GameObject title;

    //[Header("Prefab Settings")]
    //public GameObject prefab;
    //public Transform select;
    //public Button[] buttons;
    //private GameObject curButton;

    void Awake()
    {
        credit.loopPointReached += OnCreditVideoEnd;
        credit.isLooping = false;
        SetMainUI();
    }

    private void SetMainUI()
    {
        main.gameObject.SetActive(true);
        load.gameObject.SetActive(false);
        complete.gameObject.SetActive(false);
        setting.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        main.GetComponent<FadeUI>().FadeInOut(1f, 0f, 0.5f);
        Invoke("LoadSetActive", 0.6f);
    }

    private void LoadSetActive()
    {
        main.gameObject.SetActive(false);
        load.gameObject.SetActive(true);

        foreach (var saveSlotUI in saveSlotUIs)
        {
            saveSlotUI.SaveSlotInitialize();
        }
    }

    public void StartGameOff()
    {
        load.GetComponent<FadeUI>().FadeInOut(1f, 0f, 0.5f);
        Invoke("MainSetActive", 0.6f);
    }

    private void MainSetActive()
    {
        main.gameObject.SetActive(true);
        load.gameObject.SetActive(false);

        main.GetComponent<FadeUI>().FadeInOut(0f, 1f, 0.5f);
    }

    public void NewGame()
    {
        load.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName);
    }

    public void SetingON()
    {
        main.gameObject.SetActive(false);
        setting.gameObject.SetActive(true);
    }
    public void SetingOFF()
    {
        main.gameObject.SetActive(true);
        setting.gameObject.SetActive(false);
    }

    public void Credit()
    {
        creditUI.gameObject.SetActive(true);
        main.gameObject.SetActive(false);

        if (title != null)
        {
            title.gameObject.SetActive(false);
        }
        credit.loopPointReached += OnCreditVideoEnd;
        credit.Play();
    }

    private void OnCreditVideoEnd(VideoPlayer vp)
    {
        creditUI.gameObject.SetActive(false);
        main.gameObject.SetActive(true);

        if (title != null)
        {
            title.gameObject.SetActive(true);
        }
    }
    public void CompleteON()
    {
        main.gameObject.SetActive(false);
        complete.gameObject.SetActive(true);
    }
    public void CompleteOFF()
    {
        main.gameObject.SetActive(true);
        complete.gameObject.SetActive(false);
    }

    public void Back2Title()
    {
        SceneManager.LoadScene("mainTitle");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
