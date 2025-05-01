using UnityEngine;

public class ShadowSoundClip : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioArr;
    [SerializeField] public AudioSource[] audioSources;

    public void ShadowSoundOneShot(int i)
    {
        SoundManager.Instance.audioSource.PlayOneShot(audioArr[i]);
    }
}
