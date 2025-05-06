using System.Collections;
using UnityEngine;

public class GrimmPulsate : MonoBehaviour
{    
    [SerializeField] Vector3 maxScale = Vector3.one;
    [SerializeField] AudioSource heartBeatAudio;

    private float pulseSpeed;
    private float pulseDelayTime;

    private Vector3 originScale;
    private Coroutine pulsateCoroutine;

    public void StartPulsate(float _pulseSpeed, float _pulseDelayTimne)
    {
        pulseSpeed = _pulseSpeed;
        pulseDelayTime = _pulseDelayTimne;

        originScale = transform.localScale;
        pulsateCoroutine = StartCoroutine(Pulsate());
    }

    public void SetPulsateFaster(float _pulseSpeed, float _pulseDelayTime)
    {
        pulseSpeed = _pulseSpeed;
        pulseDelayTime = _pulseDelayTime;

        if (heartBeatAudio != null)
            heartBeatAudio.pitch = 1.3f;

        StopCoroutine(pulsateCoroutine);
        pulsateCoroutine = StartCoroutine(Pulsate());
    }

    private IEnumerator Pulsate()
    {
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime * pulseSpeed;
            float t = Mathf.Sin(time);
            transform.localScale = Vector3.Lerp(originScale, maxScale, t);

            if (heartBeatAudio != null && !heartBeatAudio.isPlaying && heartBeatAudio.isActiveAndEnabled)
                heartBeatAudio.PlayOneShot(heartBeatAudio.clip);

            if (t <= 0)
            {
                time = 0f;
                yield return new WaitForSeconds(pulseDelayTime);
            }

            yield return null;
        }
    }

    public void StopPulsate()
    {
        if (pulsateCoroutine != null)
        {
            StopCoroutine(pulsateCoroutine);
            pulsateCoroutine = null;
        }

        transform.localScale = originScale;
        if (heartBeatAudio != null && heartBeatAudio.isPlaying)
            heartBeatAudio.Stop();
    }
}