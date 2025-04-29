using UnityEngine;

public class BossGrimmSoundClip : MonoBehaviour
{
    [SerializeField] private AudioClip grimmAppear;
    [SerializeField] private AudioClip grimmCast1;
    [SerializeField] private AudioClip grimmTeleportIn;
    [SerializeField] private AudioClip grimmTeleportOut;

    public void GrimmCast1Sound() { SoundManager.Instance.audioSource.PlayOneShot(grimmCast1); }
    public void GrimmTeleportInSound() { SoundManager.Instance.audioSource.PlayOneShot(grimmTeleportIn); }
    public void GrimmTeleportOutSound() { SoundManager.Instance.audioSource.PlayOneShot(grimmTeleportOut); }
}
