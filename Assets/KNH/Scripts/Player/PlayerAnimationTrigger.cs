using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private SceneSaveLoadManager sceneManaer => SceneSaveLoadManager.instance;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject ghost;
    [SerializeField] private GameObject hit_effect;
    [SerializeField] private GameObject die_effect;

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //그림자 공격
            if (hit.GetComponent<Shadow>() != null)
            {
                hit.GetComponent<Shadow>().TakeDamage();
                Instantiate(hit_effect, player.attackCheck.position, Quaternion.identity);
            }
            //그림 공격
            if (hit.GetComponent<BossBase>() != null)
            {
                hit.GetComponent<BossBase>().BossTakeDamage(player.attackPower);
                Instantiate(hit_effect, player.attackCheck.position, Quaternion.identity);
            }
            //그림 박쥐 공격
            if (hit.GetComponent<BossGrimmBat>() != null)
            {
                hit.GetComponent<BossGrimmBat>().BatGetHit(player.attackPower);
                Instantiate(hit_effect, player.attackCheck.position, Quaternion.identity);
            }
            //풀 자르기
            if (hit.GetComponent<Grass>() != null)
            {
                hit.GetComponent<Grass>().CutGraas();
            }
            //구조물자르기
            if (hit.GetComponent<BreakableObject>() != null)
            {
                hit.GetComponent<BreakableObject>().Break();
            }
            //지오상자
            if (hit.GetComponent<GeoDeposit>() != null)
            {
                hit.GetComponent<GeoDeposit>().HitGeoDeposit();
            }
            //몬스터
            if (hit.GetComponent<HuskBullyController>() != null)
            {
                Vector2 knockbackDir = (hit.transform.position - transform.position).normalized;
                hit.GetComponent<HuskBullyController>().TakeDamage(player.attackPower, knockbackDir * 3);
                Instantiate(hit_effect, player.attackCheck.position, Quaternion.identity);
            }
            if (hit.GetComponent<Water>() != null)
            {
                GameObject waterSplash = Instantiate(player.WaterSplash, player.attackCheck.position, Quaternion.identity);
            }
            player.SetHPandMP(0, 10);
        }
    }

    private void FocusTrigger()
    {
        SkillManager.instance.focus.UseFocusSkill();
    }

    private void SpiritTrigger()
    {
        SkillManager.instance.spirit.UseSpiritSkill();
    }
    private void HowlingTrigger()
    {
        SkillManager.instance.howling.UseHowlingSkill();
    }
    private void DieAnimation()
    {
        GameObject obj1 = Instantiate(head, player.headPos.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Rigidbody2D rb = obj1.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(-2f, 3f), ForceMode2D.Impulse);
        GameObject obj2 = Instantiate(ghost, player.headPos.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    }
    private void ReSpawn()
    {
        BGMManager.instance.isPlayerDead = true;
        sceneManaer.StartLoadScene("Dirtmouth");
    }
}
