using UnityEngine;

public class DevRoomFadeSprite : MonoBehaviour
{
    private float fadeDuration = 0.5f;

    private void Start()
    {
        GetComponent<FadeObject>().StartSpriteFade(fadeDuration, 1f, 0f);
    }

    private void Update()
    {
        fadeDuration -= Time.deltaTime;
        if (fadeDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
