using Unity.Burst.CompilerServices;
using UnityEngine;

public class spirit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //그림자 공격
        if (collision.GetComponent<Shadow>() != null)
        {
            collision.GetComponent<Shadow>().TakeDamage();
        }
        //그림 공격
        if (collision.GetComponent<BossBase>() != null)
        {
            collision.GetComponent<BossBase>().BossTakeDamage(PlayerManager.instance.player.spiritAttackPower);
        }
        //몬스터
        if (collision.GetComponent<HuskBullyController>() != null)
        {
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            collision.GetComponent<HuskBullyController>().TakeDamage(PlayerManager.instance.player.attackPower, knockbackDir * 3);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
