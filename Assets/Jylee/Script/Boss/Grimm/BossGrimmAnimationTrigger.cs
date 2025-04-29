using UnityEngine;

public class BossGrimmAnimationTrigger : MonoBehaviour
{
    private BossGrimm boss => GetComponentInParent<BossGrimm>();
    private BossGrimmSoundClip soundClip => GetComponentInParent<BossGrimmSoundClip>();

    private void AnimTrigger()
    {
        boss.AnimationTrigger();
    }

    private void AirDashEff()
    {
        boss.AirDashEffGenerate();
        soundClip.GrimmAirDash();
    }

    private void LandEff()
    {
        boss.LandEffGenerate();
        soundClip.GrimmAirDashLand();
    }

    private void GroundDashEff()
    {
        boss.GroundDashEffGenerate();
        soundClip.GrimmGroundDashStart();
    }

    private void GroundDashing()
    {
        soundClip.GrimmGroundDashing();
    }

    private void ZeroVelocity()
    {
        boss.SetZeroVelocity();
    }

    private void GrimmCast1Sound()
    {
        soundClip.GrimmCast1();
    }

    private void GrimmTeleportInSound()
    {
        soundClip.GrimmTeleportIn();
    }

    private void GrimmTeleportOutSound()
    {
        soundClip.GrimmTeleportOut();
    }

    private void GrimmUppercutFirst()
    {
        soundClip.GrimmUppercut1();
    }

    private void GrimmUppercutSecond()
    {
        soundClip.GrimmUppercut2();
        soundClip.GrimmUppercutVoice();
    }

    private void GrimmBulletHellStart()
    {
        soundClip.GrimmCast2();
    }
}
