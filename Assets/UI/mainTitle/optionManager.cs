using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class optionManage : MonoBehaviour
{
    public GameObject main;
    public GameObject load;
    public GameObject complete;
    public GameObject setting;
    
    [Header("Credit")]
    public VideoPlayer credit;
    public GameObject creditUI;

    private void Awake()
    {
        credit.loopPointReached += OnCreditVideoEnd;
        credit.isLooping = false;
    }
    public void StartGame()
    {
        main.gameObject.SetActive(false);
        load.gameObject.SetActive(true);
    }

    public void LoadGame()
    {
        load.gameObject.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
    }

    public void Setting()
    {
        main.gameObject.SetActive(false);
        setting.gameObject.SetActive(true);
    }

    public void Credit()
    {
        creditUI.gameObject.SetActive(true);
        main.gameObject.SetActive(false);

        credit.Play();
    }

    private void OnCreditVideoEnd(VideoPlayer vp)
    {
        creditUI.gameObject.SetActive(false);
        main.gameObject.SetActive(true);
    }
    public void Complete()
    {
        main.gameObject.SetActive(false);
        complete.gameObject.SetActive(true);
    }

    public void Back2Title()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("mainTitle");
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
