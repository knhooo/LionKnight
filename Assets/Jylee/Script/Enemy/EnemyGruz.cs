using UnityEditor;
using UnityEngine;

public class EnemyGruz : MonoBehaviour
{
    [Header("기본 정보")]
    public float moveSpeed;
    public float maxHp;
    public float currentHp;
    public int facingDir { get; private set; } = 1;
    public bool facingLeft = true;
    public Collider2D attackColl;
    public float dieForceX;
    public float dieForceY;
    public float dieBoundForce;

    [Header("충돌 정보")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ceilCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float ceilCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private float upDownChangDelay;
    private bool isUp;
    private float xPower;
    private float yPower;
    private bool hasBounced;
    private float dyingDuration;
    private int stateType;
    private float deadAbsolute;

    public EnemyFx fx { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        fx = GetComponentInChildren<EnemyFx>();
        upDownChangDelay = 0.2f;
        isUp = Random.Range(1, 3) == 1 ? true : false;
        hasBounced = false;
        currentHp = maxHp;
        stateType = 1;
        deadAbsolute = 10f;
        if (Random.Range(1, 3) == 1)
        {
            ForcedFlip();
        }
    }

    void Update()
    {
        if (stateType == 1)
        {
            BossFloatPowerChange();
            Vector2 movePos = new Vector2(xPower, yPower);
            rb.linearVelocity = movePos;

            upDownChangDelay -= Time.deltaTime;

            if ((IsGroundDetected() || IsCeilDetected()) && upDownChangDelay <= 0)
            {
                upDownChangDelay = 0.5f;
                isUp = !isUp;
            }

            if (IsWallDetected())
            {
                ForcedFlip();
            }
        }
        else if(stateType == 2)
        {
            dyingDuration -= Time.deltaTime;
            deadAbsolute -= Time.deltaTime;
            if (IsGroundDetected() && dyingDuration <= 0)
            {
                if (!hasBounced)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, dieBoundForce);
                    hasBounced = true;
                    dyingDuration = 0.2f;
                }
                else
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    anim.SetTrigger("IsDead");
                    stateType = 3;
                }
            }

            if (deadAbsolute <= 0)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                anim.SetTrigger("IsDead");
                stateType = 3;
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            EnemyTakeDamage(10);
        }
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, facingLeft ? Vector2.left : Vector2.right, wallCheckDistance, whatIsGround);

    public bool IsCeilDetected() => Physics2D.Raycast(ceilCheck.position, Vector2.up, ceilCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + (Vector3)(Vector2.down * groundCheckDistance));
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)((facingLeft ? Vector2.left : Vector2.right) * wallCheckDistance));
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(ceilCheck.position, ceilCheck.position + (Vector3)(Vector2.up * ceilCheckDistance));
        
    }

    public void ForcedFlip()
    {
        FlipReverse();

        // 벽 감지 뒤집기
        Vector3 wallCheckLocalPos = wallCheck.localPosition;
        wallCheckLocalPos.x *= -1;
        wallCheck.localPosition = wallCheckLocalPos;
    }

    public void FlipReverse()
    {
        facingDir = facingDir * -1;
        facingLeft = !facingLeft;
        sr.flipX = !facingLeft;
    }

    public void BossFloatPowerChange()
    {
        xPower = facingLeft ? -moveSpeed : moveSpeed;
        yPower = isUp ? moveSpeed : -moveSpeed;
    }

    public void EnemyTakeDamage(float damage)
    {
        currentHp -= damage;
        fx.StartCoroutine("FlashFX");
        if(currentHp <= 0)
        {
            dyingDuration = 0.2f;
            stateType = 2;
            anim.SetBool("IsDying", true);
            EnemyIsDead();
        }
    }

    private void EnemyIsDead()
    {
        attackColl.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(facingLeft ? dieForceX : -dieForceX, dieForceY);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
