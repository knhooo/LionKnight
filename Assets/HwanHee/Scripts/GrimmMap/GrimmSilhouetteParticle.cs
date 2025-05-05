using UnityEngine;
using UnityEngine.EventSystems;

public class GrimmSilhouetteParticle : FadeObject
{
    [Space]
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float torque = 10f;
    [SerializeField] private float fadeOutDelay = 2f;

    private Rigidbody2D rb;
    private Vector3 startPos;
    private Vector2 moveDir;
    private bool justInstantiated = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sps[0].sprite = sprites[Random.Range(0, 3)];

        Color color = sps[0].color;
        color.a = 0;
        sps[0].color = color;

        startPos.x = transform.position.x + Random.Range(-0.1f, 0.1f);
        startPos.y = transform.position.y + Random.Range(-0.3f, 0.3f);

        transform.position = new Vector3(startPos.x, startPos.y, Random.Range(-0.3f, 0.3f));

        moveDir.x = Random.Range(-1f, 1f);
        moveDir.y = Random.Range(-1, 1f);

        speed = Random.Range(speed - 0.1f, speed + 0.1f);
    }

    private void OnEnable()
    {
        if (justInstantiated)
        {
            justInstantiated = false;
            return;
        }

        torque = Random.Range(-torque, torque);
        rb.AddTorque(torque);
        rb.AddForce(moveDir * speed, ForceMode2D.Impulse);
        Invoke("StartFadeOut", fadeOutDelay);
    }

    private void StartFadeOut()
    {
        base.StartSpriteFade(1.5f, 1f, 0f);
    }
}
