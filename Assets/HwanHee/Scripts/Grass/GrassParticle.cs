using System.Collections;
using UnityEngine;

public class GrassParticle : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sp;
    float disappearDuration = 0.5f;

    public Sprite[] sprites;

    Vector2 velocity;
    float velocityY;
    float gravity = 20f;

    float forceTime = 0.1f;
    float forceTimer;

    float lifeTime;
    float lifeTimer = 0f;

    float alpha;
    Color originColor;
    Vector3 originScale;

    bool isStartCoroutine = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();

        SetValues();
    }

    private void SetValues()
    {
        lifeTime = Random.Range(0.7f, 1.1f);

        int num = Random.Range(0, 3);
        sp.sprite = sprites[num];

        originScale = transform.localScale;

        alpha = Random.Range(0.5f, 1.1f);
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
        originColor = sp.color;

        if (alpha <= 0.8f)
            velocityY = Random.Range(4f, 6f);
        else
            velocityY = Random.Range(2f, 4f);
        velocity.x = Random.Range(-1f, 1f);
    }

    private void OnEnable()
    {
        Initialize();
        AddTorque();
    }

    private void Initialize()
    {
        rb.gravityScale = 0f;
        velocity.y = velocityY;

        forceTimer = forceTime;
        lifeTimer = lifeTime;

        isStartCoroutine = false;
        transform.localScale = originScale;
        sp.color = originColor;
    }

    void AddTorque()
    {
        rb.AddForce(new Vector2(velocity.x, 0), ForceMode2D.Impulse);
        float torque = Random.Range(200f, 300f);
        rb.AddTorque(torque);
    }

    private void Update()
    {
        AddForce();

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0 && !isStartCoroutine)
        {
            isStartCoroutine = true;
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private void AddForce()
    {
        forceTimer -= Time.deltaTime;

        if (velocity.y > 0)
        {
            if (forceTimer <= 0f)
            {
                velocity.y -= gravity * Time.deltaTime;
            }
            transform.position += (Vector3)velocity * Time.deltaTime;
        }

        else
            rb.gravityScale = 0.2f;
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < disappearDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(originColor.a, 0f, elapsedTime / disappearDuration);
            sp.color = new Color(originColor.r, originColor.g, originColor.b, alpha);
            transform.localScale = Vector3.Lerp(originScale, new Vector3(0.5f, 0.5f, 0.5f), elapsedTime / disappearDuration);

            yield return null;
        }

        sp.color = new Color(originColor.r, originColor.g, originColor.b, 0f);

        PoolManager.instance.ReturnToPool(PoolType.GrassParticle, gameObject);
    }

    public void SetPosition(Vector3 _pos)
    {
        transform.position = _pos;

        if (alpha < 0.8f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
    }
}
