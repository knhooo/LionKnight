using UnityEngine;

public class AspidProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask destroyOnLayers;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & destroyOnLayers) != 0)
        {
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Player") && collision.collider.TryGetComponent(out Player player))
        {
            player.TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & destroyOnLayers) != 0)
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            player.TakeDamage();
        }
    }
}
