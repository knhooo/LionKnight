using UnityEngine;

public class BossGruzMotherAnimationTrigger : MonoBehaviour
{
    private BossGruzMother boss => GetComponentInParent<BossGruzMother>();
    private void AnimTrigger()
    {
        boss.AnimationTrigger();
    }

}
