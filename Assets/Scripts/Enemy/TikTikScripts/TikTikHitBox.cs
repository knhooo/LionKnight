using UnityEngine;

public class TikTikHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    private BoxCollider2D col;
    private Vector2 originalSize;
    private Vector2 originalOffset;
    private TikTikController controller;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        controller = GetComponentInParent<TikTikController>();
        originalSize = col.size;
        originalOffset = col.offset;
        Debug.Log("[TikTikHitbox] Awake: Collider initialized");
    }

    private void Update()
    {
        if (controller != null && controller.IsDead() && controller.IsGrounded())
        {
            if (col.enabled)
            {
                col.enabled = false;
                Debug.Log("[TikTikHitbox] Collider disabled on death + grounded");
            }
        }
        else
        {
            if (!col.enabled)
            {
                col.enabled = true;
                Debug.Log("[TikTikHitbox] Collider re-enabled");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (controller != null && controller.IsDead()) return;

        Debug.Log($"[TikTikHitbox] Trigger enter: {other.name}");

        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            Debug.Log("[TikTikHitbox] Player hit by TikTik");
            Vector2 knockbackDir = (other.transform.position - transform.position).normalized * 5f;
            Debug.Log($"[TikTikHitbox] Damage: {damage}, Knockback: {knockbackDir}");
            player.TakeDamage();
        }
    }

    public void ActivateHitbox(bool facingRight)
    {
        if (controller != null && controller.IsDead()) return;

        float offsetX = facingRight ? 0.6f : -0.6f;
        col.size = new Vector2(1.2f, originalSize.y);
        col.offset = new Vector2(offsetX, originalOffset.y);
        Debug.Log($"[TikTikHitbox] Collider active setup: facingRight={facingRight}, offsetX={offsetX}");
    }

    public void DeactivateHitbox()
    {
        if (controller != null && controller.IsDead()) return;

        col.size = originalSize;
        col.offset = originalOffset;
        Debug.Log("[TikTikHitbox] Collider reset");
    }


}
