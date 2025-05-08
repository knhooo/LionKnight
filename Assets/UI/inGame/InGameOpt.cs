using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameOpt : MonoBehaviour
{

    public GameObject opt;
    public GameObject savecheck;
    public GameObject setOpt;

    private void Update()
    {
        Resume();
    }

    public void Resume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool optAct = !opt.gameObject.activeSelf;
            opt.gameObject.SetActive(optAct);
            if (optAct == true)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void Optionbtt()
    {
        opt.gameObject.SetActive(false);
        setOpt.gameObject.SetActive(true);
    }
    public void BacktoTitle()
    {
        SceneManager.LoadScene("mainTitle");
    }

    public void SaveCheck()
    {
        if (savecheck != null)
        {
            opt.gameObject.SetActive(false);
            savecheck.gameObject.SetActive(true);
        }

    }
}
