using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FadeObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sps;
    [SerializeField] private Light2D[] lights;
    [SerializeField] private AudioSource[] audioSources;

    [Space]
    [SerializeField] private float fadeDuration;

    [Header("Sprite")]
    [SerializeField] private float startAlpha;
    [SerializeField] private float endAlpha;
    [Header("Light")]
    [SerializeField] private float startIntensity;
    [SerializeField] private float endIntensity;
    [Header("Audio")]
    [SerializeField] private float startVolume;
    [SerializeField] private float endVolume;

    [HideInInspector] public bool isFadeEnd = false;

    private void OnValidate()
    {
        if (sps == null || sps.Length == 0)
        {
            startAlpha = -99f;
            endAlpha = -99f;
        }
        if (lights == null || lights.Length == 0)
        {
            startIntensity = -99f;
            endIntensity = -99f;
        }
        if (audioSources == null || audioSources.Length == 0)
        {
            startVolume = -99f;
            endVolume = -99f;
        }
    }

    // 인스펙터에서 설정
    public void StartSpriteFade(float _fadeDuration = -99f, float _startAlpha = -99f, float _endAlpha = -99f)
    {
        isFadeEnd = false;

        // -99일 경우 인스펙터에서 설정한 값으로 fadeDuration 설정
        if (_fadeDuration == -99f)
            _fadeDuration = fadeDuration;
        if (_startAlpha == -99f)
            _startAlpha = startAlpha;
        if (_endAlpha == -99f)
            _endAlpha = endAlpha;

        StartCoroutine(SpriteFadeInOut(_fadeDuration, _startAlpha, _endAlpha));
    }

    private IEnumerator SpriteFadeInOut(float _fadeDuration, float startAlpha, float endAlpha)
    {
        if (sps == null)
        {
            Debug.Log("Sprite 넣기");
            yield return null;
        }

        SetAlpha(startAlpha);

        float elapsed = 0;

        while (elapsed <= _fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / _fadeDuration);
            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(endAlpha);
        isFadeEnd = true;
    }

    private void SetAlpha(float alpha)
    {
        foreach (var sp in sps)
        {
            Color spriteColor = sp.color;
            spriteColor.a = alpha;
            sp.color = spriteColor;
        }
    }

    public void StartLightFade(float _fadeDuration = -99f, float _startIntensity = -99f, float _endIntensity = -99f)
    {
        isFadeEnd = false;

        if (_fadeDuration == -99f)
            _fadeDuration = fadeDuration;
        if (_startIntensity == -99f)
            _startIntensity = startIntensity;
        if (_endIntensity == -99f)
            _endIntensity = endIntensity;

        StartCoroutine(LightFadeInOut(_fadeDuration, _startIntensity, _endIntensity));
    }

    private IEnumerator LightFadeInOut(float _fadeDuration, float startIntensity, float endIntensity)
    {
        if (lights == null)
        {
            Debug.Log("Light 넣기");
            yield return null;
        }

        SetLightIntensity(startIntensity);

        float elapsed = 0f;
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;

            float intensity = Mathf.Lerp(startIntensity, endIntensity, elapsed / _fadeDuration);
            SetLightIntensity(intensity);

            yield return null;
        }

        SetLightIntensity(endIntensity);
        isFadeEnd = true;
    }

    private void SetLightIntensity(float intensity)
    {
        foreach (var light in lights)
        {
            light.intensity = intensity;
        }
    }

    public void StartBGMFade(float _fadeDuration = -99f, float _startVolume = -99f, float _targetVolume = -99f)
    {
        if (_fadeDuration == -99f)
            _fadeDuration = fadeDuration;
        if (_startVolume == -99f)
            _startVolume = startVolume;
        if (_targetVolume == -99f)
            _targetVolume = endVolume;

        StartCoroutine(VolumeFadeInOut(_fadeDuration, _startVolume, _targetVolume));
    }

    private IEnumerator VolumeFadeInOut(float _fadeDuration, float _startVolume, float _targetVolume)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float volume = Mathf.Lerp(_startVolume, _targetVolume, elapsedTime / fadeDuration);

            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.volume = volume;
            }
            yield return null;
        }

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = _targetVolume;
        }
    }
}
