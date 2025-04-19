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

    [Header("Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldownTime = 1f;
    [SerializeField] private float attackPrepTime = 0.5f;
    [SerializeField] private float deathKnockbackForce = 5f;

    private State currentState = State.Idle;
    private float stateTimer = 0f;
    private bool facingRight = true;

    private void Update()
    {
        stateTimer -= Time.deltaTime;
        HandleState();
        HandleDebugInput();
    }

    private void HandleState()
    {
        ResetAllTriggers();

        switch (currentState)
        {
            case State.Idle:
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Walk);
                }
                break;

            case State.Walk:
                animator.SetTrigger("isWalk");
                rb.linearVelocity = new Vector2((facingRight ? 1 : -1) * walkSpeed, rb.linearVelocity.y);
                if (PlayerInRange())
                {
                    rb.linearVelocity = Vector2.zero;
                    TransitionTo(State.AttackPrep);
                }
                break;

            case State.Turn:
                animator.SetTrigger("isTurn");
                Flip();
                TransitionTo(State.Idle, 0.2f);
                break;

            case State.AttackPrep:
                animator.SetTrigger("isAttack1");
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Attack);
                }
                break;

            case State.Attack:
                animator.SetTrigger("isAttack2");
                TransitionTo(State.AttackCooldown, attackCooldownTime);
                break;

            case State.AttackCooldown:
                animator.SetTrigger("isCoolDown");
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Idle);
                }
                break;

            case State.Hurt:
                animator.SetTrigger("isHurt");
                break;

            case State.DeathAir:
                animator.SetTrigger("isDeathAir");
                break;

            case State.DeathLand:
                animator.SetTrigger("isDeathLand");
                rb.linearVelocity = Vector2.zero;
                enemyCollider.enabled = false;
                break;
        }
    }

    private void ResetAllTriggers()
    {
        animator.ResetTrigger("isWalk");
        animator.ResetTrigger("isTurn");
        animator.ResetTrigger("isAttack1");
        animator.ResetTrigger("isAttack2");
        animator.ResetTrigger("isCoolDown");
        animator.ResetTrigger("isHurt");
        animator.ResetTrigger("isDeathAir");
        animator.ResetTrigger("isDeathLand");
    }

    private void TransitionTo(State newState, float timer = 1f)
    {
        currentState = newState;
        stateTimer = timer;
    }

    private bool PlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) < attackRange;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(facingRight ? 1 : -1, 1, 1);
    }

    public void Die()
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(facingRight ? -1 : 1, 1) * deathKnockbackForce, ForceMode2D.Impulse);
        TransitionTo(State.DeathAir);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == State.DeathAir && collision.collider.CompareTag("Ground"))
        {
            TransitionTo(State.DeathLand);
        }
    }

    private void HandleDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TransitionTo(State.Idle);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TransitionTo(State.Walk);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TransitionTo(State.Turn);
        if (Input.GetKeyDown(KeyCode.Q)) TransitionTo(State.AttackPrep);
        if (Input.GetKeyDown(KeyCode.W)) TransitionTo(State.Attack);
        if (Input.GetKeyDown(KeyCode.E)) TransitionTo(State.AttackCooldown);
        if (Input.GetKeyDown(KeyCode.A)) TransitionTo(State.Hurt);
        if (Input.GetKeyDown(KeyCode.Z)) Die();
        if (Input.GetKeyDown(KeyCode.X)) TransitionTo(State.DeathLand);
    }
}
