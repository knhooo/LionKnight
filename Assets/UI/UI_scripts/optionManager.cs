using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

using UnityEngine.Splines.ExtrusionShapes;
using System.Collections;


public class optionManage : MonoBehaviour
{
    [Header("버튼UI")]
    public GameObject[] btts;
    private AudioSource audioSource;
    public AudioClip bttSound;

    public GameObject main;
    public GameObject load;
    public GameObject complete;
    public GameObject setting;
    public string sceneName;
    
    [Header("세이브/로드")]
    public SaveSlotUI[] saveSlotUIs;
    public CanvasGroup saveSelect;

    [Header("크레딧")]
    public VideoPlayer credit;
    public GameObject creditUI;
    public GameObject title;

    [Header("배경화면")]
    public GameObject unactBG;
    public GameObject actBG;
    //[Header("Prefab Settings")]
    //public GameObject prefab;
    //public Transform select;
    //public Button[] buttons;
    //private GameObject curButton;

    void Awake()
    {
        BossDeadCheck();
        BttAnimator();

        credit.loopPointReached += OnCreditVideoEnd;
        credit.isLooping = false;

        SetMainUI();
    }

    private void BttAnimator()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.pitch = 2.0f;
        }

        foreach (var buttonObj in btts)
        {
            Button button = buttonObj.GetComponent<Button>();
            Animator animator = buttonObj.GetComponent<Animator>();

            if (button != null && animator != null)
            {
                button.onClick.AddListener(() => OnButtonPressed(animator));
            }
        }
    }

    private void BossDeadCheck()
    {
        if (DataManager.instance.bossDeadData.isDead)
        {
            ActivateObjects();
        }
    }

    private void ActivateObjects()
    {
        unactBG.SetActive(false);
        actBG.SetActive(true);

        this.enabled = false;
    }

    private void OnButtonPressed(Animator animator)
    {

        if (bttSound != null)
        {
            audioSource.PlayOneShot(bttSound);
        }

        animator.Play("press", 0, 0f);

        StartCoroutine(ResetAnimator(animator));
    }

    private IEnumerator ResetAnimator(Animator animator)
    {
        yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        animator.Play("Idle", 0, 0f);
        animator.Update(0);
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

        load.GetComponent<FadeUI>().FadeInOut(0f, 1f, 0.5f);

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
        foreach (var saveSlotUI in saveSlotUIs)
        {
            saveSlotUI.soulCountUI.InitializeSoulCount();
        }
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
