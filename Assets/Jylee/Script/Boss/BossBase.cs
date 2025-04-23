using UnityEngine;

public class BossBase : MonoBehaviour
{
    [Header("보스 정보")]
    public float healthPoint;
    public float damagePoint;

    [Header("충돌 정보")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("컴포넌트")]
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Collider2D cd;

    public int facingDir { get; private set; } = 1;

    public bool facingLeft = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + (Vector3)(Vector2.down * groundCheckDistance));
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

    public bool FlipController(float velocity)
    {
        // x값 높으면 -1 낮으면 1

        if (velocity > 0 && !facingLeft)
        {
            Flip();
            return true;
        }
        else if (velocity < 0 && facingLeft)
        {
            Flip();
            return true;
        }
        return false;
    }

    public void SetZeroVelocity()
    {
        rb.linearVelocity = new Vector2(0, 0);
    }
}
