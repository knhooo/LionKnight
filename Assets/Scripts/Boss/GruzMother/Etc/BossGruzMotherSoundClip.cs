using UnityEngine;

public class BossGruzMotherSoundClip : MonoBehaviour
{
    [SerializeField] private AudioClip gruzMotherWallCrash;
    [SerializeField] private AudioClip gruzMotherGrugle;
    [SerializeField] private AudioClip gruzMotherBurst;
    [SerializeField] private AudioClip bossDefeat;
    [SerializeField] private AudioClip bossExplode;
    [SerializeField] private AudioClip bossGushing;
    [SerializeField] private AudioClip bossFinalHit;

    public void GruzMotherWallCrash() { SoundManager.Instance.audioSource.PlayOneShot(gruzMotherWallCrash); }
    public void GruzMotherGrugle() { SoundManager.Instance.audioSource.PlayOneShot(gruzMotherGrugle); }
    public void GruzMotherBurst() { SoundManager.Instance.audioSource.PlayOneShot(gruzMotherBurst); }
    public void BossDefeat() { SoundManager.Instance.audioSource.PlayOneShot(bossDefeat); }
    public void BossExplode() { SoundManager.Instance.audioSource.PlayOneShot(bossExplode); }
    public void BossGushing() { SoundManager.Instance.audioSource.PlayOneShot(bossGushing); }
    public void BossFinalHit() { SoundManager.Instance.audioSource.PlayOneShot(bossFinalHit); }
}
