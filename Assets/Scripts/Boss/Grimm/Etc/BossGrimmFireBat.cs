using System.Collections;
using UnityEngine;

public class BossGrimmFireBat : MonoBehaviour
{
    public float xMoveSpeed;
    public float yMoveSpeed;
    public float duration;
    public float upPower;
    public float upDuration;

    private Rigidbody2D rb;
    private Transform playerTransform;
    [SerializeField] private AudioClip fireBatSound;

    private bool isUp;

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
        float newY;
        if (isUp)
        {
            upDuration -= Time.deltaTime;
            newY = upPower;
            if (upDuration <= 0)
            {
                isUp = false;
            }
        }
        else
        {
            // y는 천천히 플레이어 위치를 따라가게끔
            newY = Mathf.Lerp(transform.position.y, playerTransform.position.y, yMoveSpeed * Time.fixedDeltaTime);
        }

        // 새로운 속도 설정
        Vector2 newVelocity = new Vector2(xVelocity, newY - transform.position.y);
        rb.linearVelocity = newVelocity;
    }

    private IEnumerator StopVerticalMovement(Rigidbody2D rb, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }

    public void UpFireBat()
    {
        isUp = true;
        upDuration = 0.2f;
    }
}
