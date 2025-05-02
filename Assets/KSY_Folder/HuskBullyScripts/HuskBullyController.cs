using UnityEngine;

public class HuskBullyController : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walk,
        Turn,
        AttackPrep,
        Attack,
        AttackCooldown,
        Hurt,
        DeathAir,
        DeathLand
    }

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;

    [Header("Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldownTime = 1f;
    [SerializeField] private float attackPrepTime = 0.5f;
    [SerializeField] private float deathKnockbackForce = 5f;

    [Header("Combat")]
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private float damageToPlayer = 10f;
    [SerializeField] private Vector2 knockbackFromPlayer = new Vector2(5f, 3f);

    private State currentState = State.Idle;
    private float stateTimer = 0f;
    private bool facingRight = false;
    private bool hasTurnedRecently = false;
    private int currentHealth;
    private HuskBullyHitbox hitbox;

    private void Start()
    {
        hitbox = GetComponentInChildren<HuskBullyHitbox>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;
        HandleState();
    }

    private void HandleState()
    {
        if (currentState == State.Walk && IsWallAhead() && !hasTurnedRecently)
        {
            rb.linearVelocity = Vector2.zero;
            hasTurnedRecently = true;
            TransitionTo(State.Turn, 0.5f);
            return;
        }

        SetAnimatorStates();

        switch (currentState)
        {
            case State.Idle:
                if (stateTimer <= 0f)
                {
                    hasTurnedRecently = false;
                    TransitionTo(State.Walk);
                }
                break;

            case State.Walk:
                rb.linearVelocity = new Vector2((facingRight ? walkSpeed : -walkSpeed), rb.linearVelocity.y);

                if (PlayerInPatrolArea() && PlayerInRange())
                {
                    rb.linearVelocity = Vector2.zero;
                    TransitionTo(State.AttackPrep, attackPrepTime);
                }
                else if (IsWallAhead() && !hasTurnedRecently)
                {
                    rb.linearVelocity = Vector2.zero;
                    hasTurnedRecently = true;
                    TransitionTo(State.Turn, 0.5f);
                }
                else if (!IsWithinPatrolBounds())
                {
                    rb.linearVelocity = Vector2.zero;
                    hasTurnedRecently = true;
                    TransitionTo(State.Turn, 0.5f);
                }
                break;

            case State.Turn:
                if (stateTimer <= 0f)
                {
                    Flip();
                    hasTurnedRecently = false;
                    TransitionTo(State.Walk);
                }
                break;

            case State.AttackPrep:
                if (!PlayerInPatrolArea() || !PlayerInRange())
                {
                    TransitionTo(State.Walk);
                    return;
                }
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Attack, 1f);
                }
                break;

            case State.Attack:
                if (hitbox != null)
                {
                    hitbox.ResizeHitbox(facingRight);
                }

                if (stateTimer <= 0f)
                {
                    TransitionTo(State.AttackCooldown, attackCooldownTime);
                }
                break;

            case State.AttackCooldown:
                if (hitbox != null)
                {
                    hitbox.ResetHitbox();
                }

                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Walk);
                }
                break;

            case State.Hurt:
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Walk);
                }
                break;

            case State.DeathAir:
                rb.linearVelocity = Vector2.zero;
                if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
                {
                    TransitionTo(State.DeathLand);
                }
                break;

            case State.DeathLand:
                rb.linearVelocity = Vector2.zero;
                enemyCollider.enabled = false;
                break;
        }
    }

    private void SetAnimatorStates()
    {
        animator.SetBool("isWalk", currentState == State.Walk);
    }

    private bool IsWallAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = wallCheck.position;
        float rayDistance = 0.5f;
        int groundMask = LayerMask.GetMask("Ground");

        Debug.DrawRay(origin, direction * rayDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, groundMask);
        if (hit.collider != null)
        {
            Debug.Log($"[WallCheck] Hit: {hit.collider.name}, Tag: {hit.collider.tag}, Trigger: {hit.collider.isTrigger}");
            return hit.collider != enemyCollider;
        }

        return false;
    }

    private void TransitionTo(State newState, float timer = 1f)
    {
        if (currentState == newState) return;

        if (newState == State.AttackPrep) animator.SetTrigger("isAttack1");
        if (newState == State.Attack) animator.SetTrigger("isAttack2");
        if (newState == State.AttackCooldown) animator.SetTrigger("isCoolDown");
        if (newState == State.DeathAir) animator.SetTrigger("isDeathAir");
        if (newState == State.DeathLand) animator.SetTrigger("isDeathLand");
        if (newState == State.Turn) animator.SetTrigger("isTurn");

        currentState = newState;
        stateTimer = timer;
    }

    private bool PlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) < attackRange;
    }

    private bool PlayerInPatrolArea()
    {
        return player.position.x >= leftLimit.position.x && player.position.x <= rightLimit.position.x;
    }

    private bool IsWithinPatrolBounds()
    {
        return transform.position.x >= leftLimit.position.x && transform.position.x <= rightLimit.position.x;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = facingRight;

        Vector3 localPos = wallCheck.localPosition;
        localPos.x *= -1;
        wallCheck.localPosition = localPos;
    }

    public void TakeDamage(int damage, Vector2 knockbackDir)
    {
        Debug.Log($"[HuskBully] Damage Received: {damage}, Current HP: {currentHealth - damage}");

        if (currentState == State.DeathAir || currentState == State.DeathLand) return;

        currentHealth -= damage;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDir, ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            TransitionTo(State.Hurt, 0.2f);
        }
    }

    public void Die()
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(facingRight ? -1 : 1, 1) * deathKnockbackForce, ForceMode2D.Impulse);
        TransitionTo(State.DeathAir);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("[HuskBully] Player Hit");
                player.TakeDamage();
            }
        }
    }
}
