using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin noise;

    private bool isInvoking = false;

    private void Awake()
    {
        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float _shakeAmplitude, float _shakeFrequency, float _shakeDuration)
    {
        if (isInvoking)
            ForceStopShake();

        isInvoking = true;

        noise.AmplitudeGain = _shakeAmplitude;
        noise.FrequencyGain = _shakeFrequency;

        Invoke("StopShake", _shakeDuration);
    }

    private void StopShake()
    {
        isInvoking = false;

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }

    public void ForceStopShake()
    {
        CancelInvoke("StopShake");

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
    }
}
