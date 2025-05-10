using System.Collections;
using UnityEngine;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;

    private CanvasGroup[] canvasGroups;

    private void Awake()
    {
        canvasGroups = GetComponentsInChildren<CanvasGroup>(true);
    }

    public void FadeInOut(float startAlpha, float endAlpha, float _duration = -99f)
    {
        if (_duration == -99f)
            _duration = fadeDuration;

        StartCoroutine(Fade(startAlpha, endAlpha, _duration));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float _fadeDuration)
    {
        float time = 0f;

        while (time < _fadeDuration)
        {
            time += Time.deltaTime;

            foreach (var canvasGroup in canvasGroups)
            {
                if (canvasGroup.gameObject.activeSelf)
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / _fadeDuration);
            }

            yield return null;
        }

        foreach (var canvasGroup in canvasGroups)
            canvasGroup.alpha = endAlpha;
    }
}
