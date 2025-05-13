using System.Collections;
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

    [SerializeField] public GameObject hitParticle;
    [SerializeField] public GameObject dieParticle;
    public int facingDir { get; private set; } = 1;
    public ShadowSoundClip soundClip => GetComponentInParent<ShadowSoundClip>();

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
        soundClip.audioSources[0].Play();
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
            if (!soundClip.audioSources[1].isPlaying)
            {
                soundClip.audioSources[1].Play();
            }
            soundClip.audioSources[0].Stop();
        }
        else
        {
            HoverInPlace();
            if (!soundClip.audioSources[0].isPlaying)
                soundClip.audioSources[0].Play();
            soundClip.audioSources[1].Stop();
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
    }

    public void TakeDamage()
    {
        hp -= 10;
        soundClip.ShadowSoundOneShot(1);
        GameObject obj = Instantiate(hitParticle, transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        StartCoroutine("HitKnockBack");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
            collision.gameObject.GetComponent<Player>().TakeDamage();
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
            soundClip.ShadowSoundOneShot(0);
            Instantiate(dieParticle, transform.position, Quaternion.identity);
            rb.linearVelocity = new Vector2(0, 0);
            isDie = true;
            stateMachine.ChangeState(dieState);
        }
    }
}
