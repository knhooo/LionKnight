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
    public float detectionRange = 5f; // 플레이어 감지 범위
    public float moveSpeed = 2f;      // 추적 속도
    private Transform player;         // 플레이어 위치
    private bool isChasing = false;

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
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        //플레이어 방향을 바라보게 flip
        if (direction.x > 0)
            sr.flipX = false; // 오른쪽 보면 원래대로
        else if (direction.x < 0)
            sr.flipX = true;  // 왼쪽 보면 뒤집기
    }

    private void HoverInPlace()
    {
        rb.linearVelocity = Vector2.zero;
        // 여기서 약간 떠오르는 애니메이션 효과를 넣어도 좋음
    }
}
