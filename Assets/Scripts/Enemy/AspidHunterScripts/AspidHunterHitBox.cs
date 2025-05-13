using UnityEngine;

public class AspidHunterHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private CapsuleCollider2D col;
    private Vector2 originalSize;
    private Vector2 originalOffset;

    private void Awake()
    {
        col = GetComponent<CapsuleCollider2D>();
        originalSize = col.size;
        originalOffset = col.offset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            Debug.Log("[AspidHunterHitbox] Player hit by AspidHunter");
            player.TakeDamage();
        }
    }

    public void ActivateHitbox(bool facingRight)
    {
        float offsetX = facingRight ? 0.6f : -0.6f;
        col.size = new Vector2(1.2f, originalSize.y);
        col.offset = new Vector2(offsetX, originalOffset.y);
        gameObject.SetActive(true);
    }

    public void DeactivateHitbox()
    {
        col.size = originalSize;
        col.offset = originalOffset;
        gameObject.SetActive(false);
    }
}
