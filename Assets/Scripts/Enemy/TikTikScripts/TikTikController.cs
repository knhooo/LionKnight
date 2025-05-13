using UnityEngine;
using System.Collections;

public class TikTikController : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase, Hurt, DeathLand }

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public int maxHealth = 20;
    public LayerMask groundLayer;

    [Header("References")]
    public Transform player;
    public Transform groundCheck;
    public Transform wallCheck;

    [Header("Audio")]
    public AudioClip deathClip;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D[] colliders;

    private State currentState;
    private bool facingRight = true;
    private float moveDirection = 0f;
    private int currentHealth;

    private bool isDamaged = false;
    private bool colliderDisabled = false;
    [SerializeField] private int geoCount = 3;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliders = GetComponents<Collider2D>();
        animator.applyRootMotion = false;
        currentHealth = maxHealth;
    }

    private void Start()
    {
        currentState = State.Patrol;
    }

    private void Update()
    {
        animator.SetBool("isDamaged", isDamaged);

        if (currentState == State.DeathLand && !colliderDisabled)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("deathLand") && state.normalizedTime >= 1f)
            {
                foreach (var col in colliders)
                    col.enabled = false;

                colliderDisabled = true;
            }
            return;
        }

        if (currentState == State.Hurt || currentState == State.DeathLand)
            return;

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
                if (!IsGroundAhead() || IsWallAhead())
                    Flip();

                if (player.position.x < transform.position.x && facingRight)
                    Flip();
                else if (player.position.x > transform.position.x && !facingRight)
                    Flip();

                moveDirection = facingRight ? 1f : -1f;

                if (distanceToPlayer > chaseRange + 1f)
                    currentState = State.Patrol;
                break;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("[Test] K pressed - Forcing TikTik death");
            TakeDamage(currentHealth, Vector2.up);
        }
    }

    private void FixedUpdate()
    {
        if (currentState == State.DeathLand)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("deathLand")) return;
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

    public bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Die()
    {
        currentState = State.DeathLand;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        moveDirection = 0f;
        colliderDisabled = false;

        animator.Play("deathLand", 0, 0);
        animator.SetBool("isLand", true);
        CreateGeo();

        if (SoundManager.Instance != null && deathClip != null)
        {
            SoundManager.Instance.audioSource.PlayOneShot(deathClip);
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackDir)
    {
        if (currentState == State.DeathLand) return;

        currentHealth -= damage;
        rb.linearVelocity = Vector2.zero;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            TransitionTo(State.Hurt, 0.2f);
        }
    }

    public bool IsDead()
    {
        return currentState == State.DeathLand;
    }

    private void TransitionTo(State state, float duration)
    {
        StartCoroutine(StateTransition(state, duration));
    }

    private IEnumerator StateTransition(State state, float duration)
    {
        currentState = state;
        isDamaged = true;
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(duration);
        isDamaged = false;
        animator.SetBool("isDamaged", false);
        currentState = State.Patrol;
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
    public void CreateGeo()
    {
        for (int i = 0; i < geoCount; i++)
        {
            Geo geo = PoolManager.instance.Spawn(PoolType.Geo, transform.position, Quaternion.identity).GetComponent<Geo>();
        }
    }
}
