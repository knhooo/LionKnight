using UnityEngine;

public class PlayerSoundClip : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioArr;

    public void PlayerSoundOneShot(int i)
    {
        SoundManager.Instance.audioSource.PlayOneShot(audioArr[i]);
    }
}
