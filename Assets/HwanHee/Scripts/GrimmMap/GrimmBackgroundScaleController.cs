using System.Collections;
using System.Net.Security;
using UnityEngine;

public class GrimmBackgroundScaleController : MonoBehaviour
{
    [SerializeField] float scaleSpeed;
    [SerializeField] float delayTime;
    [SerializeField] Vector3 maxScale = Vector3.one;
    [SerializeField] AudioSource heartBeatAudio;

    public void StartPulsate()
    {
        StartCoroutine(Pulsate());
    }

    private IEnumerator Pulsate()
    {
        float time = 0f;
        Vector3 originScale = transform.localScale;
        while (true)
        {
            time += Time.deltaTime * scaleSpeed;
            float t = Mathf.Sin(time);
            transform.localScale = Vector3.Lerp(originScale, maxScale, t);

            if (heartBeatAudio != null && !heartBeatAudio.isPlaying && heartBeatAudio.isActiveAndEnabled)
                heartBeatAudio.Play();

            if (t <= 0)
            {
                time = 0f;
                yield return new WaitForSeconds(delayTime);
            }

            yield return null;
        }
    }
}