using UnityEngine;

public class BossGrimmAnimationTrigger : MonoBehaviour
{
    private BossGrimm boss => GetComponent<BossGrimm>();

    private void AniEndTrigger()
    {
        boss.AnimationTrigger();
    }
}
