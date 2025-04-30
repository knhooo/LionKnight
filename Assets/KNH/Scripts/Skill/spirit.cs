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
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
