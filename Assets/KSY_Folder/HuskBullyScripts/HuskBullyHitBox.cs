using UnityEngine;

public class HuskBullyHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private BoxCollider2D col;
    private Vector2 originalSize;
    private Vector2 originalOffset;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        originalSize = col.size;
        originalOffset = col.offset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("[Hitbox] Player Hit by HuskBullyHitbox");
                player.TakeDamage();
            }
        }
    }

    public void ResizeHitbox(bool facingRight)
    {
        float offsetX = facingRight ? 0.6f : -0.6f;
        col.size = new Vector2(1.2f, originalSize.y);
        col.offset = new Vector2(offsetX, originalOffset.y);
    }

    public void ResetHitbox()
    {
        col.size = originalSize;
        col.offset = originalOffset;
    }
}
