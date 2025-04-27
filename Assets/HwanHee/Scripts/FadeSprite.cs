using System.Collections;
using UnityEngine;

public class FadeSprite : MonoBehaviour
{
    [SerializeField] private float fadeOutDuration;

    SpriteRenderer sp;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    public IEnumerator StartFadeOut()
    {
        Color originColor = sp.color;
        float elapsed = 0;

        while (elapsed <= fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            sp.color = Color.Lerp(originColor, new Color(0f, 0f, 0f, 0f), elapsed / fadeOutDuration);
            yield return null;
        }

        sp.color = new Color(0f, 0f, 0f, 0f);
    }
}
