using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    private Canvas sceneFaderCanvas;
    private Image fadeImage;

    private bool isSceneChange = false;

    private void Awake()
    {
        sceneFaderCanvas = GetComponent<Canvas>();
        fadeImage = sceneFaderCanvas.GetComponentInChildren<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void FadeToScene()
    {
        if (!isSceneChange)
        {
            isSceneChange = true;
            sceneFaderCanvas.GetComponent<Canvas>().gameObject.SetActive(true);
            sceneFaderCanvas.gameObject.SetActive(true);
            StartCoroutine(FadeOutIn());
        }
    }

    IEnumerator FadeOutIn()
    {
        yield return StartCoroutine(Fade(0f, 1f));
        SceneSaveLoadManager.instance.LoadScene();

        yield return StartCoroutine(Fade(1f, 0f));
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color _color = fadeImage.color;

        while (time < fadeDuration)
        {
            _color.a = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            fadeImage.color = _color;
            time += Time.deltaTime;
            yield return null;
        }

        _color.a = endAlpha;
        fadeImage.color = _color;

        if (fadeImage.color.a == 0f)
        {
            sceneFaderCanvas.GetComponent<Canvas>().gameObject.SetActive(false);
            isSceneChange = false;
        }
    }
}