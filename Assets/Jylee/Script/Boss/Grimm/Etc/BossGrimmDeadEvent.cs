using System.Collections;
using UnityEngine;

public class BossGrimmDeadEvent : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f; // 전체 효과 지속 시간
    [SerializeField] private float maxScale = 1.5f; // 최대 크기 배율

    [SerializeField] private GameObject centerCircle;
    private SpriteRenderer ccSpriteRenderer;
    private Vector3 ccOriginalScale;
    private float timer;
    private bool centerDone;

    [SerializeField] private GameObject circlePrefab; // 퍼질 원 프리팹
    [SerializeField] private float spreadDuration = 5f; // 전체 지속 시간
    [SerializeField] private float spawnInterval = 0.1f; // 발사 간격
    [SerializeField] private float burstSpeed = 4f;
    [SerializeField] private float circleMaxScale;
    [SerializeField] private float circleMinScale;
    private Coroutine burstCoroutine;

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

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("V");
            CancelCenterCircleGenerate();
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
    }

    public IEnumerator BurstRoutine()
    {
        float elapsed = 0f;

        while (elapsed < spreadDuration)
        {
            SpawnBurst();
            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
        }

        burstCoroutine = null;
        Destroy(gameObject);
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
        Vector2 randomDir = Random.insideUnitCircle.normalized;

        GameObject circle = Instantiate(circlePrefab, transform.position, Quaternion.identity);
        float scale = Random.Range(circleMinScale, circleMaxScale);
        circle.transform.localScale = new Vector3(scale, scale, 1f);

        Rigidbody2D rb = circle.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = randomDir * burstSpeed;

        Destroy(circle, 0.5f); // 각 원 수명
    }
}
