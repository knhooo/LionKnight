using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : Singleton<BGMManager>
{
    private float fadeDuration = 0.5f;

    private AudioSource[] audioSources;
    [HideInInspector] public float currentBGMVolume;
    [HideInInspector] public float newBGMVolume;
    [HideInInspector] public bool playImmediately;
    private AudioClip[] newAudioClips;

    protected override void Awake()
    {
        base.Awake();

        audioSources = GetComponents<AudioSource>();
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

    public void SetNewBGM(AudioClip newClip)
    {
        audioSources[0].clip = newClip;
    }

    public void SetNewBGMs(AudioClip[] _newAudioClips)
    {
        newAudioClips = _newAudioClips;
    }

    public void BGMFadeOut(float BGMVolume)
    {
        StartCoroutine(VolumeFadeInOut(audioSources[0].volume, BGMVolume));
    }

    public void BGMFadeOut()
    {
        StartCoroutine(VolumeFadeInOut(audioSources[0].volume, currentBGMVolume));
    }

    private void PlayNewBGM()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (i < newAudioClips.Length)
            {
                audioSources[i].clip = newAudioClips[i];
                if (playImmediately)
                    audioSources[i].Play();
            }
            else
                audioSources[i].clip = null;
        }
        if (playImmediately)
            StartCoroutine(VolumeFadeInOut(0f, newBGMVolume));
    }

    private IEnumerator VolumeFadeInOut(float startVolume, float endVolume)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / fadeDuration);

            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.volume = volume;
            }
            yield return null;
        }

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = endVolume;
        }

        // volume 0 : 새로운 bgm 재생
        if (audioSources[0].volume == 0 && newAudioClips != null)
        {
            yield return new WaitForSeconds(0.5f);
            PlayNewBGM();
        }
    }


    public void SetValues(float _currentBGMVolume, float _newBGMVolume, bool _playImmediately)
    {
        currentBGMVolume = _currentBGMVolume;
        newBGMVolume = _newBGMVolume;
        playImmediately = _playImmediately;
    }
}
