using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : Singleton<BGMManager>
{
    private float fadeDuration = 0.5f;

    private AudioSource[] audioSources;
    [HideInInspector] public float CurrentBGMVolume;
    [HideInInspector] public float NewBGMVolume;
    private AudioClip[] newAudioClips;

    protected override void Awake()
    {
        base.Awake();

        audioSources = GetComponents<AudioSource>();
    }

    public void SetNewBGM(AudioClip[] _newAudioClips)
    {
        newAudioClips = _newAudioClips;
    }

    public void BGMFadeOut()
    {
        StartCoroutine(VolumeFadeInOut(audioSources[0].volume, CurrentBGMVolume));
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
        if (audioSources[0].volume == 0)
        {
            yield return new WaitForSeconds(0.5f);
            PlayNewBGM();
        }
    }

    private void PlayNewBGM()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (i < newAudioClips.Length)
            {
                audioSources[i].clip = newAudioClips[i];
                audioSources[i].Play();
            }
            else
                audioSources[i].clip = null;
        }
        StartCoroutine(VolumeFadeInOut(0f, NewBGMVolume));
    }
}
