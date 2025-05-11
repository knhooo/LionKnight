using Unity.Burst.CompilerServices;
using UnityEngine;

public class spirit : MonoBehaviour
{
    private int damage;

    private void Start()
    {
        damage = PlayerManager.instance.player.playerData.spell_Damage;
    }

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
            collision.GetComponent<BossBase>().BossTakeDamage(damage);
        }
        //그림 박쥐 공격
        if (collision.GetComponent<BossGrimmBat>() != null)
        {
            collision.GetComponent<BossGrimmBat>().BatGetHit(damage);
        }
        //몬스터
        if (collision.GetComponent<HuskBullyController>() != null)
        {
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            collision.GetComponent<HuskBullyController>().TakeDamage(damage, knockbackDir * 3);
        }
        else if (collision.GetComponent<AspidHunterController>() != null)
        {
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            collision.GetComponent<AspidHunterController>().TakeDamage(damage, knockbackDir * 3);
        }
        else if (collision.GetComponent<TikTikController>() != null)
        {
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            collision.GetComponent<TikTikController>().TakeDamage(damage, knockbackDir * 3);
        }

        if (collision.GetComponent<EnemyGruz>() != null)
        {
            collision.GetComponent<EnemyGruz>().EnemyTakeDamage(damage);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
