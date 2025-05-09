using UnityEngine;

public class BossGrimmSoundClip : MonoBehaviour
{
    [SerializeField] private AudioClip grimmGreeting;
    [SerializeField] private AudioClip grimmAppear;
    [SerializeField] private AudioClip grimmCast1;
    [SerializeField] private AudioClip grimmCast2;
    [SerializeField] private AudioClip grimmEvade;
    [SerializeField] private AudioClip grimmTeleportIn;
    [SerializeField] private AudioClip grimmTeleportOut;
    [SerializeField] private AudioClip grimmAirDash;
    [SerializeField] private AudioClip grimmAirDashLand;
    [SerializeField] private AudioClip grimmGroundDashStart;
    [SerializeField] private AudioClip grimmGroundDashing;
    [SerializeField] private AudioClip grimmUppercut1;
    [SerializeField] private AudioClip grimmUppercut2;
    [SerializeField] private AudioClip grimmUppercutVoice;
    [SerializeField] private AudioClip grimmSpikesGrounded;
    [SerializeField] private AudioClip grimmSpikesShootUp;
    [SerializeField] private AudioClip grimmSpikesShrivelBack;
    [SerializeField] private AudioClip grimmBalloonDeflate;
    [SerializeField] private AudioClip grimmBalloonShootingFireLoop;
    [SerializeField] private AudioClip grimmExplodeIntoBats;
    [SerializeField] private AudioClip grimmBatsCircling;
    [SerializeField] private AudioClip grimmBatsReform;
    [SerializeField] private AudioClip grimmLongDefeat;
    [SerializeField] private AudioClip grimmFinalHit;
    [SerializeField] private AudioClip grimmDefeatBgm;
    [SerializeField] private AudioClip grimmScream;
    [SerializeField] private AudioClip grimmStun;

    public void GrimmGreeting() { SoundManager.Instance.audioSource.PlayOneShot(grimmGreeting); }
    public void GrimmAppear() { SoundManager.Instance.audioSource.PlayOneShot(grimmAppear); }
    public void GrimmCast1() { SoundManager.Instance.audioSource.PlayOneShot(grimmCast1); }
    public void GrimmCast2() { SoundManager.Instance.audioSource.PlayOneShot(grimmCast2); }
    public void GrimmEvade() { SoundManager.Instance.audioSource.PlayOneShot(grimmEvade); }
    public void GrimmTeleportIn() { SoundManager.Instance.audioSource.PlayOneShot(grimmTeleportIn); }
    public void GrimmTeleportOut() { SoundManager.Instance.audioSource.PlayOneShot(grimmTeleportOut); }
    public void GrimmAirDash() { SoundManager.Instance.audioSource.PlayOneShot(grimmAirDash); }
    public void GrimmAirDashLand() { SoundManager.Instance.audioSource.PlayOneShot(grimmAirDashLand); }
    public void GrimmGroundDashStart() { SoundManager.Instance.audioSource.PlayOneShot(grimmGroundDashStart); }
    public void GrimmGroundDashing() { SoundManager.Instance.audioSource.PlayOneShot(grimmGroundDashing); }
    public void GrimmUppercut1() { SoundManager.Instance.audioSource.PlayOneShot(grimmUppercut1); }
    public void GrimmUppercut2() { SoundManager.Instance.audioSource.PlayOneShot(grimmUppercut2); }
    public void GrimmUppercutVoice() { SoundManager.Instance.audioSource.PlayOneShot(grimmUppercutVoice); }
    public void GrimmSpikesGrounded() { SoundManager.Instance.audioSource.PlayOneShot(grimmSpikesGrounded); }
    public void GrimmSpikesShootUp() { SoundManager.Instance.audioSource.PlayOneShot(grimmSpikesShootUp); }
    public void GrimmSpikesShrivelBack() { SoundManager.Instance.audioSource.PlayOneShot(grimmSpikesShrivelBack); }
    public void GrimmBalloonDeflate() { SoundManager.Instance.audioSource.PlayOneShot(grimmBalloonDeflate); }
    public void GrimmBalloonShootingFireLoop() { SoundManager.Instance.audioSource.PlayOneShot(grimmBalloonShootingFireLoop); }
    public float GrimmBalloonShootingFireLoopLength() { return grimmBalloonShootingFireLoop.length; }
    public void GrimmExplodeIntoBats() { SoundManager.Instance.audioSource.PlayOneShot(grimmExplodeIntoBats); }
    public void GrimmBatsCircling() { SoundManager.Instance.audioSource.PlayOneShot(grimmBatsCircling); }
    public void GrimmBatsReform() { SoundManager.Instance.audioSource.PlayOneShot(grimmBatsReform); }
    public void GrimmLongDefeat() { SoundManager.Instance.audioSource.PlayOneShot(grimmLongDefeat); }
    public float GrimmLongDefeatLength() { return grimmLongDefeat.length; }
    public void GrimmFinalHit() { SoundManager.Instance.audioSource.PlayOneShot(grimmFinalHit); }
    public void GrimmDefeatBgm() { SoundManager.Instance.audioSource.PlayOneShot(grimmDefeatBgm); }

    public float GrimmDefeatBgmLength() { return grimmDefeatBgm.length; }
    public void GrimmScream() { SoundManager.Instance.audioSource.PlayOneShot(grimmScream); }
    public void GrimmStun() { SoundManager.Instance.audioSource.PlayOneShot(grimmStun); }
}
