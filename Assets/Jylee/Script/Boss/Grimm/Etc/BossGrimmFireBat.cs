using UnityEngine;

public class BossGrimmFireBat : MonoBehaviour
{
    public float xMoveSpeed;
    public float yMoveSpeed;
    public float duration;

    private Rigidbody2D rb;
    private Transform playerTransform;
    [SerializeField] private AudioClip fireBatSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerTransform == null)
        {
            playerTransform = gameObject.transform;
        }

        SoundManager.Instance.audioSource.PlayOneShot(fireBatSound);
    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }

        // x는 현재 방향으로 고정
        float xVelocity = transform.localScale.x > 0 ? xMoveSpeed : -xMoveSpeed;

        // y는 천천히 플레이어 위치를 따라가게끔
        float newY = Mathf.Lerp(transform.position.y, playerTransform.position.y, yMoveSpeed * Time.fixedDeltaTime);

        // 새로운 속도 설정
        Vector2 newVelocity = new Vector2(xVelocity, newY - transform.position.y);
        rb.linearVelocity = newVelocity;
    }
}
