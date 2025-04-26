using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin noise;

    private float shakeTimer;

    private void Awake()
    {
        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                StopShake();
            }
        }
    }

    public void ShakeCamera(float _shakeAmplitude, float _shakeFrequency, float _shakeDuration)
    {
        noise.AmplitudeGain = _shakeAmplitude;
        noise.FrequencyGain = _shakeFrequency;
        shakeTimer = _shakeDuration;
    }

    private void StopShake()
    {
        noise.AmplitudeGain = 0f;
    }
}
