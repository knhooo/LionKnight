using System.Collections;
using UnityEngine;

public class FadeInOutObject : MonoBehaviour
{
    private float fadeDuration = 0.1f;

    private Material material;
    private Color originColor;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        originColor = material.color;
    }

    public void StartFadeInOut(float _fadeDuration, float startAlpha, float finalAlpha)
    {
        fadeDuration = _fadeDuration;
        StartCoroutine(FadeInOutCoroutine(startAlpha, finalAlpha));
    }

    private IEnumerator FadeInOutCoroutine(float startAlpha, float finalAlpha)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, finalAlpha, timer / fadeDuration);

            Color color = originColor;
            color.a = alpha;
            material.color = color;

            yield return null;
        }

        Color finalColor = originColor;
        finalColor.a = finalAlpha;
        material.color = finalColor;

        if (finalAlpha == 0f)
            gameObject.SetActive(false);
    }
}
