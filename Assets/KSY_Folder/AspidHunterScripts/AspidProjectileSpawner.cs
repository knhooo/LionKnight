using UnityEngine;

public class AspidProjectileSpawner : MonoBehaviour
{
    private Vector2 lastDirection = Vector2.right;

    [Header("Dependencies")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;

    public void CacheDirection(Vector2 baseDirection)
    {
        lastDirection = baseDirection;
    }

    public void FireFromAnimation()
    {
        Vector2[] directions = new Vector2[]
        {
            lastDirection + Vector2.up,
            lastDirection,
            lastDirection + Vector2.down
        };

        foreach (Vector2 dir in directions)
        {
            GameObject spit = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            if (spit.TryGetComponent(out Rigidbody2D rb))
            {
                rb.gravityScale = 0.8f;
                rb.linearVelocity = dir.normalized * projectileSpeed;
            }

            if (spit.TryGetComponent(out TrailRenderer trail))
            {
                trail.time = 0.3f;
                trail.startWidth = 0.1f;
                trail.endWidth = 0f;
                trail.material = new Material(Shader.Find("Sprites/Default"));
                trail.startColor = new Color(1f, 0.9f, 0.2f, 0.6f);
                trail.endColor = new Color(1f, 0.9f, 0.2f, 0f);
            }
        }

    }
}
