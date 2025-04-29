using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameOpt : MonoBehaviour
{

    public GameObject opt;

    private void Update()
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
    public void BacktoTitle()
    {
        SceneManager.LoadScene("mainTitle");
    }
}
