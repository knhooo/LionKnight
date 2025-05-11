using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SceneFaderCanvas : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    private Canvas sceneFaderCanvas;
    [SerializeField] private Image fadeImage;

    private bool isSceneChange = false;

    private void Awake()
    {
        sceneFaderCanvas = GetComponent<Canvas>();

        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void FadeToScene()
    {
        if (sceneFaderCanvas == null)
            sceneFaderCanvas = GetComponent<Canvas>();

        if (!isSceneChange)
        {
            isSceneChange = true;
            sceneFaderCanvas.GetComponent<Canvas>().gameObject.SetActive(true);
            sceneFaderCanvas.gameObject.SetActive(true);
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        yield return StartCoroutine(Fade(0f, 1f));

        SceneSaveLoadManager.instance.LoadSceneAfterFadeOut();

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
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
            isSceneChange = false;
        }
    }
}