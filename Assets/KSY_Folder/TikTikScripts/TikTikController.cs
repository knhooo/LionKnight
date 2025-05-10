using UnityEngine;

public class TikTikController : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase }

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public LayerMask groundLayer;

    [Header("References")]
    public Transform player;
    public Transform groundCheck;
    public Transform wallCheck;

    private Rigidbody2D rb;
    private Animator animator;

    private State currentState;
    private bool facingRight = true;
    private float moveDirection = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
    }

    private void Start()
    {
        currentState = State.Patrol;

        if (rb.bodyType != RigidbodyType2D.Dynamic)
            Debug.LogWarning("TikTik Rigidbody2D must be Dynamic.");

        if (moveSpeed == 0)
            Debug.LogWarning("TikTik moveSpeed is 0.");
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);
        moveDirection = 0f;

        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer <= chaseRange)
                    currentState = State.Chase;
                break;

            case State.Patrol:
                if (!IsGroundAhead() || IsWallAhead())
                    Flip();

                moveDirection = facingRight ? 1f : -1f;

                if (distanceToPlayer <= chaseRange)
                    currentState = State.Chase;
                break;

            case State.Chase:
                if (player.position.x < transform.position.x && facingRight)
                    Flip();
                else if (player.position.x > transform.position.x && !facingRight)
                    Flip();

                moveDirection = facingRight ? 1f : -1f;

                if (distanceToPlayer > chaseRange + 1f)
                    currentState = State.Patrol;
                break;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
    }

    private bool IsGroundAhead()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);
    }

    private bool IsWallAhead()
    {
        return Physics2D.Raycast(wallCheck.position, facingRight ? Vector2.right : Vector2.left, 0.5f, groundLayer);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck)
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 1f);

        if (wallCheck)
        {
            Vector3 dir = facingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + dir * 0.5f);
        }
    }
}
