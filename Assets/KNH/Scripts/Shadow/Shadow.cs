using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public SpriteRenderer sr { get; private set; }
    #endregion

    [Header("Stats")]
    [SerializeField] private int hp = 20;
    public float detectionRange = 5f; // 플레이어 감지 범위
    public float moveSpeed = 2f;      // 추적 속도
    private Transform player;         // 플레이어 위치
    private bool isChasing = false;

    [Header("넉백 정보")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] public float knockbackDuration;
    protected bool isKnocked;
    protected bool isDie = false;
    public int facingDir { get; private set; } = 1;

    #region States

    public ShadowStateMachine stateMachine { get; private set; }
    public ShadowIdleState idleState { get; private set; }
    public ShadowAwakeState awakeState { get; private set; }
    public ShadowDieState dieState { get; private set; }
    #endregion

    private void Awake()
    {
        stateMachine = new ShadowStateMachine();
        idleState = new ShadowIdleState(this, stateMachine, "Idle");
        awakeState = new ShadowAwakeState(this, stateMachine, "Awake");
        dieState = new ShadowDieState(this, stateMachine, "Die");
    }

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // 태그로 플레이어 찾기

        stateMachine.Initialize(idleState);
    }
    private void Update()
    {
        if (isKnocked || isDie)
        {
            return;
        }
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isChasing && distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            HoverInPlace();
        }

        Die();
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        //플레이어 방향을 바라보게 flip
        if (direction.x > 0)
        {
            facingDir = -1;
            sr.flipX = false; // 오른쪽 보면 원래대로
        }
        else if (direction.x < 0)
        {
            facingDir = 1;
            sr.flipX = true;  // 왼쪽 보면 뒤집기
        }
    }

    private void HoverInPlace()
    {
        rb.linearVelocity = Vector2.zero;
        // 여기서 약간 떠오르는 애니메이션 효과를 넣어도 좋음
    }

    public void TakeDamage()
    {
        hp -= 10;

        StartCoroutine("HitKnockBack");
    }

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;

        rb.linearVelocity = new Vector2(knockbackDirection.x * facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;

    }
    public void Die()
    {
        if (hp <= 0)
        {
            rb.linearVelocity = new Vector2(0, 0);
            isDie = true;
            stateMachine.ChangeState(dieState);
        }
    }
}
