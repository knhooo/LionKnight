using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float _shakeAmplitude, float _shakeFrequency, float _shakeDuration)
    {
        noise.AmplitudeGain = _shakeAmplitude;
        noise.FrequencyGain = _shakeFrequency;

        Invoke("StopShake", _shakeDuration);
    }

    private void StopShake()
    {
        noise.AmplitudeGain = 0f;
    }
}
