using UnityEngine;

public class BossGrimmAnimationTrigger : MonoBehaviour
{
    private BossGrimm boss => GetComponentInParent<BossGrimm>();

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
}
