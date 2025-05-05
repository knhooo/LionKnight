using System.Collections;
using UnityEngine;

public class GrimmFadeSprite : FadeObject
{
    public void StartFadeInOut(float fadeDuration, float targetAlpha)
    {
        base.StartSpriteFade(fadeDuration / 2f, 0f, targetAlpha);

        StartCoroutine(StartFadeOut(fadeDuration, targetAlpha));
    }

    private IEnumerator StartFadeOut(float fadeDuration, float targetAlpha)
    {
        yield return new WaitForSeconds(fadeDuration / 2f);
        base.StartSpriteFade(fadeDuration / 2f, targetAlpha, 0f);
    }
}
