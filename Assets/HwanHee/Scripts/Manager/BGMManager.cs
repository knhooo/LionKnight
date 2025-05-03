using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : Singleton<BGMManager>
{
    private float fadeDuration = 0.5f;

    private AudioSource[] audioSources;
    private AudioClip[] newAudioClips;
    [HideInInspector] public float currentBGMTargetVolume;
    [HideInInspector] public float newBGMTargetVolume;

    private bool isBGMChange = false;

    protected override void Awake()
    {
        base.Awake();

        audioSources = GetComponents<AudioSource>();
    }

    public void SetBGM(AudioClip[] _newAudioClips, float _currentBGMVolume, float _newBGMVolume)
    {
        if (_newAudioClips.Length > 0)
        {
            isBGMChange = true;
            newAudioClips = _newAudioClips;
        }
        else
        {
            isBGMChange = false;
            newAudioClips = null;
        }

        currentBGMTargetVolume = _currentBGMVolume;
        newBGMTargetVolume = _newBGMVolume;
    }

    public void BGMFadeOut(float _startVolume = -99f, float _targetVolume = -99f)
    {
        if (_startVolume == -99f)
        {
            _startVolume = audioSources[0].volume;
        }

        if (_targetVolume == -99f)
        {
            if (!isBGMChange)
                _targetVolume = currentBGMTargetVolume;
            else
                _targetVolume = 0f;
        }

        StartCoroutine(VolumeFadeInOut(_startVolume, _targetVolume));
    }

    public void ChangeBGM(bool isWait)
    {
        if (!isBGMChange)
            return;

        if (isWait)
            Invoke("ApplyBGMChange", 0.5f);
        else
            ApplyBGMChange();
    }

    private void ApplyBGMChange()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (i < newAudioClips.Length)
                audioSources[i].clip = newAudioClips[i];
            else
                audioSources[i].clip = null;

            if (audioSources[i].clip != null)
            {
                audioSources[i].Stop();
                audioSources[i].Play();
            }
        }
        BGMFadeIn();
    }

    public void BGMFadeIn(float _startVolume = -99f, float _targetVolume = -99f)
    {
        if (!isBGMChange)
            return;

        if (_startVolume == -99f)
        {
            _startVolume = 0f;
        }
        if (_targetVolume == -99f)
        {
            _targetVolume = newBGMTargetVolume;
        }

        StartCoroutine(VolumeFadeInOut(_startVolume, _targetVolume));
    }

    private IEnumerator VolumeFadeInOut(float _startVolume, float _targetVolume)
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

    public void PlayBGM()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip != null)
            {
                audioSource.volume = 1f;
                audioSource.Play();
            }
        }
    }

    public void StopBGM()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip != null)
            {
                audioSource.Stop();
            }
        }
    }
}
