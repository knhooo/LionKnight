using UnityEngine;
using System.Collections.Generic;

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
<<<<<<< Updated upstream
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
=======
>>>>>>> Stashed changes

    [Header("Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldownTime = 1f;
    [SerializeField] private float attackPrepTime = 0.5f;
    [SerializeField] private float deathKnockbackForce = 5f;
    [SerializeField] private float wallCheckDistance = 3f;
    [SerializeField] private LayerMask wallLayer;

    private State currentState = State.Idle;
    private float stateTimer = 0f;
<<<<<<< Updated upstream
    private bool facingRight = false;
    private bool hasTurnedRecently = false;
=======
    private bool facingRight = false; // HuskBully 기본 방향 좌측
    private bool isTurning = false;

    private readonly Dictionary<State, string> stateTriggers = new()
    {
        { State.Walk, "isWalk" },
        { State.Turn, "isTurn" },
        { State.AttackPrep, "isAttack1" },
        { State.Attack, "isAttack2" },
        { State.AttackCooldown, "isCoolDown" },
        { State.DeathAir, "isDeathAir" },
        { State.DeathLand, "isDeathLand" }
    };
>>>>>>> Stashed changes

    private void Update()
    {
        stateTimer -= Time.deltaTime;
        HandleState();
    }

    private void HandleState()
    {
<<<<<<< Updated upstream
        if (IsWallAhead() && !hasTurnedRecently && currentState == State.Walk)
        {
            rb.linearVelocity = Vector2.zero;
            hasTurnedRecently = true;
            TransitionTo(State.Turn, 0.5f);
            return;
        }
        SetAnimatorStates();

=======
>>>>>>> Stashed changes
        switch (currentState)
        {
            case State.Idle:
                if (stateTimer <= 0f)
                {
<<<<<<< Updated upstream
                    hasTurnedRecently = false;
=======
                    isTurning = false;
>>>>>>> Stashed changes
                    TransitionTo(State.Walk);
                }
                break;

            case State.Walk:
                rb.linearVelocity = new Vector2((facingRight ? walkSpeed : -walkSpeed), rb.linearVelocity.y);
                if (PlayerInRange())
                {
                    rb.linearVelocity = Vector2.zero;
                    TransitionTo(State.AttackPrep, attackPrepTime);
<<<<<<< Updated upstream
=======
                }
                else if (IsWallAhead() && !isTurning)
                {
                    rb.linearVelocity = Vector2.zero;
                    isTurning = true;
                    TransitionTo(State.Turn, 0.5f);
>>>>>>> Stashed changes
                }
                else if (IsWallAhead() && !hasTurnedRecently)
                {
                    rb.linearVelocity = Vector2.zero;
                    hasTurnedRecently = true;
                    TransitionTo(State.Turn, 0.5f);
                }
                if (PlayerInRange())
                {
                    rb.linearVelocity = Vector2.zero;
                    TransitionTo(State.AttackPrep, attackPrepTime);
                }

                break;

            case State.Turn:
<<<<<<< Updated upstream
                if (stateTimer <= 0f)
                {
                    Flip();
                    hasTurnedRecently = false;
                    TransitionTo(State.Walk);
                }
                break;

            case State.AttackPrep:
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Attack, 0.4f);
                }
                break;

            case State.Attack:
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.AttackCooldown, attackCooldownTime);
                }
                break;

            case State.AttackCooldown:
=======
>>>>>>> Stashed changes
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Idle);
                }
                break;

<<<<<<< Updated upstream
            case State.DeathAir:
                rb.linearVelocity = Vector2.zero;
=======
            case State.AttackPrep:
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Attack, 0.4f);
                }
                break;

            case State.Attack:
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.AttackCooldown, attackCooldownTime);
                }
                break;

            case State.AttackCooldown:
                if (stateTimer <= 0f)
                {
                    TransitionTo(State.Idle);
                }
                break;

            case State.DeathAir:
>>>>>>> Stashed changes
                break;

            case State.DeathLand:
                rb.linearVelocity = Vector2.zero;
                enemyCollider.enabled = false;
                break;
        }
    }

<<<<<<< Updated upstream
    private void SetAnimatorStates()
    {
        animator.SetBool("isWalk", currentState == State.Walk);
    }

    private bool IsWallAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Vector2 origin = wallCheck.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, 0.1f);
        return hit.collider != null && hit.collider != enemyCollider;
    }

=======
    private bool IsWallAhead()
    {
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        Debug.DrawRay(wallCheck.position, direction * wallCheckDistance, Color.red); // 시각화
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, direction, wallCheckDistance, wallLayer);
        return hit.collider != null;
    }


>>>>>>> Stashed changes
    private void ResetAllTriggers()
    {
        animator.ResetTrigger("isAttack1");
        animator.ResetTrigger("isAttack2");
        animator.ResetTrigger("isCoolDown");
        animator.ResetTrigger("isDeathAir");
        animator.ResetTrigger("isDeathLand");
        animator.ResetTrigger("isTurn");
    }

    private void TransitionTo(State newState, float timer = 1f)
    {
        if (currentState == newState) return;

        ResetAllTriggers();
<<<<<<< Updated upstream

        if (newState == State.AttackPrep) animator.SetTrigger("isAttack1");
        if (newState == State.Attack) animator.SetTrigger("isAttack2");
        if (newState == State.AttackCooldown) animator.SetTrigger("isCoolDown");
        if (newState == State.DeathAir) animator.SetTrigger("isDeathAir");
        if (newState == State.DeathLand) animator.SetTrigger("isDeathLand");
        if (newState == State.Turn) animator.SetTrigger("isTurn");

=======
        if (stateTriggers.TryGetValue(newState, out var trigger))
        {
            animator.SetTrigger(trigger);
        }
>>>>>>> Stashed changes
        currentState = newState;
        stateTimer = timer;
    }

    private bool PlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) < attackRange &&
               player.position.x >= leftLimit.position.x && player.position.x <= rightLimit.position.x;
    }

<<<<<<< Updated upstream
    void Flip()
=======
    public void Flip()
>>>>>>> Stashed changes
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = facingRight;

        Vector3 localPos = wallCheck.localPosition;
        localPos.x *= -1;
        wallCheck.localPosition = localPos;
    }


    public void Die()
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(facingRight ? -1 : 1, 1) * deathKnockbackForce, ForceMode2D.Impulse);
        rb.linearVelocity = Vector2.zero;
        enemyCollider.enabled = false;
        TransitionTo(State.DeathLand);
    }


}

