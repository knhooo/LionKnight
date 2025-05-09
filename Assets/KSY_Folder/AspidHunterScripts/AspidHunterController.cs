using UnityEngine;

public class AspidHunterController : MonoBehaviour
{
    private enum State { Idle, Patrol, AttackPrep, Attack, Cooldown, Turn, Death }

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private AspidProjectileSpawner projectileSpawner;
    [SerializeField] private Transform leftBound;
    [SerializeField] private Transform rightBound;

    [Header("Settings")]
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float verticalDetectRange = 3f;
    [SerializeField] private float attackPrepTime = 0.5f;
    [SerializeField] private float attackCooldownTime = 1f;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float patrolChangeDirectionTime = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int maxHealth = 3;

    private State currentState = State.Idle;
    private float stateTimer = 0f;
    private float patrolTimer = 0f;
    private bool facingRight = false;
    private int currentHealth;

    private void Start()
    {
        facingRight = false;
        patrolTimer = Random.Range(2f, patrolChangeDirectionTime);
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) Die();
        stateTimer -= Time.deltaTime;
        patrolTimer -= Time.deltaTime;
        HandleState();
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Patrol:
                HandlePatrol();
                break;
            case State.AttackPrep:
                HandleAttackPrep();
                break;
            case State.Attack:
                HandleAttack();
                break;
            case State.Cooldown:
                HandleCooldown();
                break;
            case State.Turn:
                HandleTurn();
                break;
            case State.Death:
                HandleDeath();
                break;
        }
    }

    private void HandleIdle()
    {
        if (PlayerInLineOfSight())
            TransitionTo(State.AttackPrep, attackPrepTime);
        else if (stateTimer <= 0f)
            TransitionTo(State.Patrol);
    }

    private void HandlePatrol()
    {
        rb.linearVelocity = new Vector2((facingRight ? 1 : -1) * patrolSpeed, rb.linearVelocity.y);

        if (PlayerInLineOfSight())
        {
            rb.linearVelocity = Vector2.zero;
            TransitionTo(State.AttackPrep, attackPrepTime);
            return;
        }

        float posX = transform.position.x;
        if ((facingRight && posX >= rightBound.position.x) || (!facingRight && posX <= leftBound.position.x))
        {
            TransitionTo(State.Turn);
        }
    }

    private void HandleAttackPrep()
    {
        rb.linearVelocity = Vector2.zero;

        if (!PlayerInLineOfSight())
        {
            TransitionTo(State.Patrol);
            return;
        }

        if (stateTimer <= 0f)
            TransitionTo(State.Attack);
    }

    private void HandleAttack()
    {
        // 플레이어 방향으로 즉시 전환
        Vector2 toPlayer = player.position - transform.position;
        if ((toPlayer.x > 0 && !facingRight) || (toPlayer.x < 0 && facingRight))
        {
            Flip();
        }

        animator.SetTrigger("isAttack");
        projectileSpawner.CacheDirection(facingRight ? Vector2.right : Vector2.left);
        TransitionTo(State.Cooldown, attackCooldownTime);
    }

    private void HandleCooldown()
    {
        if (!PlayerInLineOfSight())
        {
            TransitionTo(State.Patrol);
            return;
        }

        if (stateTimer <= 0f)
            TransitionTo(State.Idle);
    }

    private void HandleTurn()
    {
        animator.SetTrigger("isTurn");
        Flip();
        patrolTimer = Random.Range(2f, patrolChangeDirectionTime);
        TransitionTo(State.Idle, 0.5f);
    }

    private bool deathHandled = false;

    private void HandleDeath()
    {
        if (deathHandled) return;

        deathHandled = true;
        animator.SetTrigger("isDeath");
        rb.linearVelocity = Vector2.zero;
        enemyCollider.enabled = false;
        Destroy(gameObject, 3f);
    }

    private void TransitionTo(State newState, float timer = 0f)
    {
        currentState = newState;
        stateTimer = timer;
    }

    private bool PlayerInRange()
    {
        Vector2 toPlayer = player.position - transform.position;
        return Mathf.Abs(toPlayer.x) < detectRange && Mathf.Abs(toPlayer.y) < verticalDetectRange;
    }

    private bool PlayerInLineOfSight()
    {
        if (!PlayerInRange()) return false;

        Vector2 origin = transform.position;
        Vector2 target = player.position;
        Vector2 direction = (target - origin).normalized;
        float distance = Vector2.Distance(target, origin);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, groundLayer);

        return !(hit.collider != null && hit.collider.gameObject != player.gameObject);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? -1 : 1, 1, 1);
    }

    public void FireProjectileFromAnimation()
    {
        projectileSpawner.FireFromAnimation();
    }

    public void TakeDamage(int damage, Vector2 knockbackDir)
    {
        Debug.Log($"[AspidHunter] TakeDamage called with damage: {damage}, knockback: {knockbackDir}");
        if (currentState == State.Death) return;

        currentHealth -= damage;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDir, ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("isHit");
            TransitionTo(State.Idle, 0.2f);
        }
    }



public void Die()
{
    TransitionTo(State.Death);
}

private void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("[AspidHunter] OnTriggerEnter2D with: " + other.name);
    if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
    {
        player.TakeDamage();
    }
}

#if UNITY_EDITOR
private void OnDrawGizmos()
{
    if (player == null) return;

    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, player.position);

    if (leftBound != null && rightBound != null)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(leftBound.position, rightBound.position);
    }
}
#endif
}
