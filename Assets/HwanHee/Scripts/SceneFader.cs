using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    bool isSceneChange = false;

    private void Awake()
    {
        fadeImage = GetComponentInChildren<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);

        DontDestroyOnLoad(this);
    }

    public void FadeToScene(string sceneName)
    {
        if (!isSceneChange)
        {
            isSceneChange = true;
            StartCoroutine(FadeOutIn(sceneName));
        }
    }

    IEnumerator FadeOutIn(string sceneName)
    {
        yield return StartCoroutine(Fade(0f, 1f));
        SceneManager.LoadScene(sceneName);
        yield return null;

        yield return StartCoroutine(Fade(1f, 0f));
        isSceneChange = false;
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
    }
}
