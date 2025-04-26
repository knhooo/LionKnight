using System.Collections;
using UnityEngine;

public class GrimmLight : MonoBehaviour
{
    private float fadeDuration = 0.1f;

    private Material material;
    private Color startColor;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        startColor = material.color;

        Color color = startColor;
        color.a = 0f;
        material.color = color;
    }

    public void StartFadeIn(float _fadeDuration)
    {
        fadeDuration = _fadeDuration;
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            Color color = startColor;
            color.a = alpha;
            material.color = color;

            yield return null;
        }

        Color finalColor = startColor;
        material.color = finalColor;
    }
}
