using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameOpt : MonoBehaviour
{

    public GameObject opt;
    public GameObject savecheck;
    public GameObject setOpt;

    public static InGameOpt instance;

    [SerializeField] private AudioClip[] titleUIBGM;

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

        UIInitialize();
    }

    private void UIInitialize()
    {
        opt.gameObject.SetActive(false);
        savecheck.gameObject.SetActive(false);
        setOpt.gameObject.SetActive(false);
    }

    private void Update()
    {
        Resume();
    }

    public void Resume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetTimeScale();

            if (setOpt.gameObject.activeSelf)
            {
                setOpt.gameObject.SetActive(false);
            }
            else if (savecheck.gameObject.activeSelf)
            {
                savecheck.gameObject.SetActive(false);
            }
            else
            {
                bool optAct = !opt.gameObject.activeSelf;
                opt.gameObject.SetActive(optAct);
            }
        }
    }

    public void Contunue()
    {
        SetTimeScale();
        opt.gameObject.SetActive(false);
    }

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
        BGMManager.instance.SetBGM(titleUIBGM, 0f, 1f);
        SceneSaveLoadManager.instance.StartLoadScene("UI_mainTitle");
        SetTimeScale();
    }

    private void SetTimeScale()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
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
