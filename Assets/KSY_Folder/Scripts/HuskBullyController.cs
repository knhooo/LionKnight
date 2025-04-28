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
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;

    [Header("Settings")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldownTime = 1f;
    [SerializeField] private float attackPrepTime = 0.5f;
    [SerializeField] private float deathKnockbackForce = 5f;

    private State currentState = State.Idle;
    private float stateTimer = 0f;
    private bool facingRight = false;
    private bool hasTurnedRecently = false;

    private void Update()
    {
        stateTimer -= Time.deltaTime;
        HandleState();
    }

    private void HandleState()
    {
        if (IsWallAhead() && !hasTurnedRecently && currentState == State.Walk)
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
                if (PlayerInRange())
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
                if (PlayerInRange())
                {
                    rb.linearVelocity = Vector2.zero;
                    TransitionTo(State.AttackPrep, attackPrepTime);
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
                rb.linearVelocity = Vector2.zero;
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
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, 0.1f);
        return hit.collider != null && hit.collider != enemyCollider;
    }

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
        return Vector2.Distance(transform.position, player.position) < attackRange &&
               player.position.x >= leftLimit.position.x && player.position.x <= rightLimit.position.x;
    }

    void Flip()
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

