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
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
