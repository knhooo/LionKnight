using Unity.Burst.CompilerServices;
using UnityEngine;

public class BossGrimmBat : MonoBehaviour
{
    public bool isBoss;

    private Transform player;
    private Transform combineTransform;
    private bool isCombine;

    public float moveSpeed = 3f;
    public float baseHeight = 2f;
    public float moveRadius = 4f;
    public float changeTargetTime = 1.5f;

    public float turnCooldown = 0.5f; // 방향전환 쿨타임
    public float turnLerpSpeed = 5f; // 방향 부드럽게 전환

    public float minY = -2f; // 바닥 제한
    public float maxY = 6f;  // 천장 제한

    public EnemyFx fx { get; private set; }

    private Vector2 targetPos;
    private float timer;
    private float turnTimer;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private float lastDirX = 0f;
    private Vector2 moveDir;

    private GameObject bossObj;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        PickNewTarget();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        fx = GetComponentInChildren<EnemyFx>();
        lastDirX = Mathf.Sign(targetPos.x - transform.position.x);
        spriteRenderer.flipX = lastDirX < 0;
        moveDir = (targetPos - (Vector2)transform.position).normalized;

        isCombine = false;
    }

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;
        turnTimer += Time.deltaTime;

        if (isCombine)
        {
            if (Vector2.Distance(transform.position, targetPos) < 0.1f) // 오차 허용
            {
                Destroy(gameObject);
            }
            Vector2 currentPos = transform.position;
            Vector2 desiredDir = (targetPos - currentPos).normalized;
            transform.position += (Vector3)(desiredDir * moveSpeed * Time.deltaTime);
        }
        else
        {
            if (timer >= changeTargetTime)
            {
                PickNewTarget();
                timer = 0f;
            }

            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        Vector2 currentPos = transform.position;
        Vector2 desiredDir = (targetPos - currentPos).normalized;

        moveDir = Vector2.Lerp(moveDir, desiredDir, Time.deltaTime * turnLerpSpeed).normalized;

        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);

        // 방향 전환 체크
        float dirX = Mathf.Sign(moveDir.x);
        if (dirX != lastDirX && turnTimer >= turnCooldown)
        {
            lastDirX = dirX;
            turnTimer = 0f;

            if (animator != null)
                animator.SetTrigger("IsTurn");

            if (spriteRenderer != null)
                spriteRenderer.flipX = dirX < 0;
        }

        // Y값 범위 제한
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }

    private void PickNewTarget()
    {
        Vector2 playerPos = player.position;

        float targetX = playerPos.x + Random.Range(-moveRadius, moveRadius);
        float playerY = playerPos.y;
        float targetY = playerY + baseHeight;

        if (playerY > transform.position.y)
        {
            targetY = playerY - Random.Range(1f, 2f);
        }
        else
        {
            targetY += Random.Range(-1f, 1f);
        }

        // 목표 Y도 제한 걸어주기
        targetY = Mathf.Clamp(targetY, minY, maxY);

        targetPos = new Vector2(targetX, targetY);
    }

    public void CombineTrigger(Transform transform)
    {
        isCombine = true;
        targetPos = transform.position;

        // 방향 전환 체크
        float dirX = Mathf.Sign(transform.position.x);
        if (dirX != lastDirX)
        {
            lastDirX = dirX;

            if (animator != null)
                animator.SetTrigger("IsTurn");

            if (spriteRenderer != null)
                spriteRenderer.flipX = dirX < 0;
        }
    }

    public void GetMainBossObj(GameObject obj)
    {
        bossObj = obj;
    }

    public void BatGetHit(float damage)
    {
        fx.StartCoroutine("FlashFX");
        if (bossObj != null)
        {
            bossObj.GetComponent<BossBase>().BossTakeDamage(damage);
        }
    }
}
