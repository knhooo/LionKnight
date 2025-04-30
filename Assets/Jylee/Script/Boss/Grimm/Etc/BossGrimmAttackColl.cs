using UnityEngine;

public class BossGrimmAttackColl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌!");
        Debug.Log(collision.tag);
        if (collision.CompareTag("Player"))
        {
            Debug.Log("player");
            collision.GetComponent<Player>()?.TakeDamage();
        }
    }
}
