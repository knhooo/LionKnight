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


        }
    }
}
