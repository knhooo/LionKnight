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
    }

    private void LandEff()
    {
        boss.LandEffGenerate();
    }

    private void GroundDashEff()
    {
        boss.GroundDashEffGenerate();
    }

    private void ZeroVelocity()
    {
        boss.SetZeroVelocity();
    }

    private void GrimmCast1Sound()
    {
        soundClip.GrimmCast1Sound();
    }

    private void GrimmTeleportInSound()
    {
        soundClip.GrimmTeleportInSound();
    }

    private void GrimmTeleportOutSound()
    {
        soundClip.GrimmTeleportOutSound();
    }
}
