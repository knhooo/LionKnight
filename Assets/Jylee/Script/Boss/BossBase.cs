using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    [Header("보스 정보")]
    public float healthPoint;
    public float currentHealthPoint;
    public float damagePoint;
    public bool isInvincible;

    [Header("충돌 정보")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform ceilCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected float ceilCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("피격 정보")]
    [SerializeField] protected float shakeAmplitude;
    [SerializeField] protected float shakeFrequency;
    [SerializeField] protected float shakeDuration;
    [SerializeField] protected CinemachineCamera cineCam;
    [SerializeField] protected GameObject hitCrack;

    [Header("컴포넌트")]
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Collider2D cd;
    public EnemyFx fx { get; private set; }

    public int facingDir { get; private set; } = 1;

    public bool facingLeft = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
        fx = GetComponentInChildren<EnemyFx>();
        cineCam = FindFirstObjectByType<CinemachineCamera>();

        currentHealthPoint = healthPoint;
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, facingLeft ? Vector2.left : Vector2.right, wallCheckDistance, whatIsGround);

    public bool IsCeilDetected() => Physics2D.Raycast(ceilCheck.position, Vector2.up, ceilCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + (Vector3)(Vector2.down * groundCheckDistance));

        if(wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)((facingLeft ? Vector2.left : Vector2.right) * wallCheckDistance));
        }

        if (ceilCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(ceilCheck.position, ceilCheck.position + (Vector3)(Vector2.up * ceilCheckDistance));
        }
    }

    protected virtual void Update()
    {

    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingLeft = !facingLeft;
        sr.flipX = facingLeft;
    }

    public void FlipReverse()
    {
        facingDir = facingDir * -1;
        facingLeft = !facingLeft;
        sr.flipX = !facingLeft;
    }

    public bool FlipController(float velocity, bool reverse)
    {
        // x값 높으면 -1 낮으면 1

        if (velocity > 0 && !facingLeft)
        {
            if (reverse)
            {
                FlipReverse();
            }
            else
            {
                Flip();
            }
            return true;
        }
        else if (velocity < 0 && facingLeft)
        {
            if (reverse)
            {
                FlipReverse();
            }
            else
            {
                Flip();
            }
            return true;
        }
        return false;
    }

    public void SetZeroVelocity()
    {
        rb.linearVelocity = new Vector2(0, 0);
    }

    public void BossTakeDamage(float damage)
    {
        if (isInvincible)
        {
            return;
        }

        currentHealthPoint -= damage;
        fx.StartCoroutine("FlashFX");
        BossCheckHealthPoint();
    }

    protected virtual void BossCheckHealthPoint()
    {
        
    }    
}
