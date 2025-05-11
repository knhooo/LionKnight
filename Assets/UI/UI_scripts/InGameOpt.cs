using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameOpt : MonoBehaviour
{

    public GameObject opt;
    public GameObject savecheck;
    public GameObject setOpt;

    public static InGameOpt instance;

    private bool isPaused = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        Resume();
    }

    public void Resume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;

            if (setOpt.gameObject.activeSelf)
            {
                setOpt.gameObject.SetActive(false);
            }
            else
            {
                bool optAct = !opt.gameObject.activeSelf;
                opt.gameObject.SetActive(optAct);
            }
        }
    }

    public void Contunue() => opt.gameObject.SetActive(false);

    public void Optionbtt()
    {
        if (opt != null)
        {
            opt.gameObject.SetActive(false);
            setOpt.gameObject.SetActive(true);
        }
    }

    public void SaveCheck()
    {
        if (savecheck != null)
        {
            opt.gameObject.SetActive(false);
            savecheck.gameObject.SetActive(true);
        }
    }
    public void SaveCheckYes()
    {
        SceneManager.LoadScene("UI_mainTitle");
        //데이터세이브
    }
    public void SaveCheckNo()
    {
        if (savecheck != null)
        {
            opt.gameObject.SetActive(true);
            savecheck.gameObject.SetActive(false);
        }
    }
    public void Back()
    {
        if (setOpt != null)
        {
            opt.gameObject.SetActive(true);
            setOpt.gameObject.SetActive(false);
        }
    }
}
