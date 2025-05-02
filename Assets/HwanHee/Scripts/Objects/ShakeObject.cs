using System.Collections;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    [Header("Shake")]
    [SerializeField] float amount;
    [SerializeField] float frequency;
    [SerializeField] protected float duration;

    protected virtual void Shake(Transform target, bool isHorizontalShake)
    {
        StartCoroutine(ShakeCoroutine(target, isHorizontalShake));
    }

    private IEnumerator ShakeCoroutine(Transform target, bool isHorizontalShake)
    {
        if (target == null)
        {
            Debug.Log("컴포넌트 없음");
            yield return null;
        }

        Vector3 originalPos = target.transform.position;

        float timer = duration;
        float elapsed = 0f;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            elapsed += Time.deltaTime;

            float offset = Mathf.Sin(elapsed * 2f * Mathf.PI * frequency) * amount;

            if (isHorizontalShake)
                target.transform.position = originalPos + new Vector3(offset, 0f, 0f);
            else
                target.transform.position = originalPos + new Vector3(0f, -offset, 0f);
            yield return null;
        }

        target.transform.position = originalPos;
    }
}
