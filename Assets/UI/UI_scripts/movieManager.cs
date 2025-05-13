using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class movieManager : MonoBehaviour
{
    public VideoPlayer first;
    public VideoPlayer second;
    float secondTime;

    private void Awake()
    {
        first.gameObject.GetComponent<VideoPlayer>();
        second.gameObject.GetComponent<VideoPlayer>();

        first.loopPointReached += OnFirstVideoEnd;
        second.loopPointReached += OnSecondVideoEnd;
    }
    private void Update()
    {
        if(first.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                first.gameObject.SetActive(false);
                StartCoroutine(Intro2());
            }
        }

        if (second.isPlaying)
        {
            secondTime += Time.deltaTime;

            if (secondTime > 1f && Input.GetKeyDown(KeyCode.Space))
            {
                second.gameObject.SetActive(false);
                SceneManager.LoadScene("UI_mainTitle");
            }
        }
    }

    private void OnFirstVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(Intro2());
    }

    private void OnSecondVideoEnd(VideoPlayer vp)
    {
        second.gameObject.SetActive(false);
        SceneManager.LoadScene("UI_mainTitle");
    }

    IEnumerator Intro2()
    {
        yield return new WaitForSeconds(0.5f);
        
        first.gameObject.SetActive(false);
        second.gameObject.SetActive(true);
    }
}
