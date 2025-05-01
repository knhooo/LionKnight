using UnityEngine;

public class ShadowSoundClip : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioArr;

    public void ShadowSoundOneShot(int i)
    {
        SoundManager.Instance.audioSource.PlayOneShot(audioArr[i]);
    }
}
