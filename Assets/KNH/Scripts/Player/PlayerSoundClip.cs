using UnityEngine;

public class PlayerSoundClip : MonoBehaviour
{
    [SerializeField] public AudioClip[] audioClips;
    [SerializeField] public AudioSource[] audioSources;

    public void PlayerSoundOneShot(int i)
    {
        SoundManager.Instance.audioSource.PlayOneShot(audioClips[i]);
    }
}
