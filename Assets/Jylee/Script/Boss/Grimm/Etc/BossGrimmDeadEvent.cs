using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class BossGrimmDeadEvent : MonoBehaviour
{
    [Header("사망시 큰원")]
    [SerializeField] private float duration = 1.0f; // 전체 효과 지속 시간
    [SerializeField] private float maxScale = 1.5f; // 최대 크기 배율

    [SerializeField] private GameObject centerCircle;
    private SpriteRenderer ccSpriteRenderer;
    private Vector3 ccOriginalScale;
    private float timer;
    private bool centerDone;

    [Header("퍼저나가는 파티클 1")]
    [SerializeField] private GameObject circlePrefab; // 퍼질 원 프리팹
    [SerializeField] private float spreadDuration = 5f; // 전체 지속 시간
    [SerializeField] private float spawnInterval = 0.1f; // 발사 간격
    [SerializeField] private float burstSpeed = 4f;
    [SerializeField] private float circleMaxScale;
    [SerializeField] private float circleMinScale;
    [SerializeField] private float circleBurstRadius;
    private Coroutine burstCoroutine;

    [Header("퍼저나가는 파티클 2")]
    [SerializeField] private GameObject smokePrefab; // 가스 프리팹
    [SerializeField] private float smokeMaxScale;
    [SerializeField] private float smokeMinScale;
    [SerializeField] private float smokeSpeed = 4f;

    [Header("폭발")]
    [SerializeField] private float burstRadius = 4f;
    [SerializeField] private float particleCount = 100f;
    [SerializeField] private bool abTrigger = false;

    private float spawnTimer = 0f;

    private void Start()
    {
        ccSpriteRenderer = centerCircle.GetComponent<SpriteRenderer>();
        ccOriginalScale = centerCircle.transform.localScale;
        timer = 0f;
        centerDone = false;
    }

    private void Update()
    {
        if (!centerDone)
        {
            CenterCircleDisappear();
        }
        else
        {
        }
    }

    public void StartCenterCircleGenerate()
    {
        if (burstCoroutine == null)
        {
            burstCoroutine = StartCoroutine(BurstRoutine());
        }
    }

    public void CancelCenterCircleGenerate()
    {
        if (burstCoroutine != null)
        {
            StopCoroutine(burstCoroutine);
            burstCoroutine = null;
        }

        if (abTrigger)
        {
            for (int i = 0; i < particleCount; i++)
            {
                SpawnParticle(circlePrefab, burstSpeed);
                SpawnParticle(smokePrefab, smokeSpeed);
            }
        }
    }

    public IEnumerator BurstRoutine()
    {
        float elapsed = 0f;

        while (elapsed < spreadDuration)
        {
            SpawnBurst();
            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
            Debug.Log(elapsed);
        }

        burstCoroutine = null;
        // Destroy(gameObject);

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < particleCount; i++)
        {
            SpawnParticle(circlePrefab, burstSpeed);
            SpawnParticle(smokePrefab, smokeSpeed);
        }
    }

    private void CenterCircleDisappear()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // 크기 증가
        float scale = Mathf.Lerp(1f, maxScale, t);
        ccSpriteRenderer.transform.localScale = ccOriginalScale * scale;

        // 투명도 감소
        Color color = ccSpriteRenderer.color;
        color.a = Mathf.Lerp(1f, 0f, t);
        ccSpriteRenderer.color = color;

        // 끝나면 제거
        if (timer >= duration)
        {
            Destroy(centerCircle);
            StartCenterCircleGenerate();
            centerDone = true;
        }
    }

    private void SpawnBurst()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(0f, circleBurstRadius);
        Vector3 spawnPos = transform.position + (Vector3)offset;

        GameObject circle = Instantiate(circlePrefab, spawnPos, Quaternion.identity);
        float scale = Random.Range(circleMinScale, circleMaxScale);
        circle.transform.localScale = new Vector3(scale, scale, 1f);

        // 방향 벡터 (정규화 후 속도 적용)
        Vector2 direction = offset.normalized;
        Rigidbody2D rb = circle.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * burstSpeed;
        }

        Destroy(circle, 0.5f); // 각 원 수명

        if(smokePrefab != null)
        {
            angle = Random.Range(0f, Mathf.PI * 2);
            offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(0f, circleBurstRadius);
            spawnPos = transform.position + (Vector3)offset;

            GameObject smoke = Instantiate(smokePrefab, spawnPos, Quaternion.identity);
            scale = Random.Range(smokeMinScale, smokeMaxScale);
            smoke.transform.localScale = new Vector3(scale, scale, 1f);

            // 방향 벡터 (정규화 후 속도 적용)
            direction = offset.normalized;
            Rigidbody2D smokeRb = smoke.GetComponent<Rigidbody2D>();
            if (smokeRb != null)
            {
                smokeRb.linearVelocity = direction * smokeSpeed;
            }

            Destroy(smoke, 0.5f); // 각 원 수명
        }
    }

    private void SpawnParticle(GameObject prefab, float speed)
    {
        // 랜덤한 각도 및 위치 계산
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(0f, burstRadius);

        Vector3 spawnPos = transform.position + (Vector3)offset;

        GameObject particle = Instantiate(prefab, spawnPos, Quaternion.identity);

        // 방향 벡터 (정규화 후 속도 적용)
        Vector2 direction = offset.normalized;
        Rigidbody2D rb = particle.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        Destroy(particle, 0.5f);
    }
}
