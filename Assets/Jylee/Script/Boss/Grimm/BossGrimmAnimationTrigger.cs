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

    // 공격 판정 콜라이더 On Off
    private void GrimmFrontAttackEnable()
    {
        boss.frontAttackPoint.GetComponent<Collider2D>().enabled = true;
    }

    private void GrimmFrontAttackDisable()
    {
        boss.frontAttackPoint.GetComponent<Collider2D>().enabled = false;
    }

    private void GrimmUppercutAttackEnable()
    {
        boss.uppercutAttackPoint.GetComponent<Collider2D>().enabled = true;
    }

    private void GrimmUppercutAttackDisable()
    {
        boss.uppercutAttackPoint.GetComponent<Collider2D>().enabled = false;
    }

    private void GrimmAirDashAttackEnable()
    {
        boss.airDashAttackPoint.GetComponent<Collider2D>().enabled = true;
    }

    private void GrimmAirDashAttackDisable()
    {
        boss.airDashAttackPoint.GetComponent<Collider2D>().enabled = false;
    }

    private void GrimmFrontDashAttackEnable()
    {
        boss.frontDashAttackPoint.GetComponent<Collider2D>().enabled = true;
    }

    private void GrimmFrontDashAttackDisable()
    {
        boss.frontDashAttackPoint.GetComponent<Collider2D>().enabled = false;
    }

    private void GrimmBodyCollEnable()
    {
        boss.bossBodyPoint.GetComponent<Collider2D>().enabled = true;
    }

    private void GrimmBodyCollDisable()
    {
        boss.bossBodyPoint.GetComponent<Collider2D>().enabled = false;
    }

    private void GrimmSmallBodyCollEnable()
    {
        boss.bossSmallBodyPoint.GetComponent<Collider2D>().enabled = true;
    }

    private void GrimmSmallBodyCollDisable()
    {
        boss.bossSmallBodyPoint.GetComponent<Collider2D>().enabled = false;
    }

    private void GroggyEff()
    {
        boss.BossGroggy();
    }
}
