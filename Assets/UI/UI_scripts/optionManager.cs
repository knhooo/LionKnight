using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;


public class optionManage : MonoBehaviour
{
    public GameObject main;
    public GameObject load;
    public GameObject complete;
    public GameObject setting;
    public string sceneName;

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
    }

    public void StartGame()
    {
        main.gameObject.SetActive(false);
        load.gameObject.SetActive(true);
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
