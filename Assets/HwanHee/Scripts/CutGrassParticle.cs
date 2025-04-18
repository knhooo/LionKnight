using System.Collections;
using UnityEngine;

public class CutGrassParticle : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sp;

    float lifeTime;
    float lifeTimer;
    float disappearDuration = 0.3f;
    Color originColor;
    Vector3 originScale;

    float alpha;

    [SerializeField] float impulseX;
    [SerializeField] float minImpulseY;
    [SerializeField] float maxImpulseY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();

        lifeTime = Random.Range(0.7f, 1.1f);

        originScale = transform.localScale;

        alpha = Random.Range(0.5f, 1.1f);
        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, alpha);
        originColor = sp.color;
    }

    private void OnEnable()
    {
        Initialize();
        ApplyImpulse();
    }

    private void Initialize()
    {
        lifeTimer = lifeTime;

        rb.linearVelocity = new Vector2(0f, 0f);
        rb.gravityScale = 1f;

        transform.localScale = originScale;
        sp.color = originColor;
    }

    void ApplyImpulse()
    {
        float forceX = Random.Range(-impulseX, impulseX);
        float forceY = Random.Range(minImpulseY, maxImpulseY);
        rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);

        float torque = Random.Range(100f, 200f);
        rb.AddTorque(torque);
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
        {
            StartCoroutine(FadeOutCoroutine());
        }

        if (rb.linearVelocityY <= 0f)
        {
            rb.gravityScale = 0.3f;
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < disappearDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(originColor.a, 0f, elapsedTime / disappearDuration);
            sp.color = new Color(originColor.r, originColor.g, originColor.b, alpha);
            transform.localScale = Vector3.Lerp(originScale, Vector3.zero, elapsedTime / disappearDuration);

            yield return null;
        }

        sp.color = new Color(originColor.r, originColor.g, originColor.b, 0f);
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 _pos)
    {
        transform.position = _pos;

        if (alpha < 0.8f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);
    }
}
