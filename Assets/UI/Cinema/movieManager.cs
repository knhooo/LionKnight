using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class movieManager : MonoBehaviour
{
    public VideoPlayer first;
    public VideoPlayer second;
    float secondTime;
    private VideoPlayer.EventHandler OnFirstVideoEnd;

    private void Awake()
    {
        first.gameObject.GetComponent<VideoPlayer>();
        second.gameObject.GetComponent<VideoPlayer>();
    }
    private void Update()
    {
        if (first.isPlaying)
        {
            first.loopPointReached += OnFirstVideoEnd;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Intro2());
            }
        }
        
        if (second.isPlaying)
        {
            secondTime += Time.deltaTime;

            if (secondTime > 1f && Input.GetKeyDown(KeyCode.Space))
            {
                second.gameObject.SetActive(false);
                UnityEngine.SceneManagement.SceneManager.LoadScene("mainTitle");
            }
        }
    }

    IEnumerator Intro2()
    {
        yield return new WaitForSeconds(1f);
        first.gameObject.SetActive(false);
        second.gameObject.SetActive(true);
    }
}
