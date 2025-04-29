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

        //foreach (Button button in buttons)
        //{
        //    button.onClick.AddListener(() => SpawnPrefab(button));
        //}
    }
    //public void SpawnPrefab(Button clickedButton)
    //{
    //    if (curButton != null)
    //    {
    //        Destroy(curButton);
    //    }
    //    if (prefab != null && select != null)
    //    {
    //        curButton = Instantiate(prefab, select.position, select.rotation);
    //    }
    //}

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
