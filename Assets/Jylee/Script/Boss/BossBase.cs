using UnityEngine;

public class BossBase : MonoBehaviour
{
    [Header("보스 정보")]
    public float healthPoint;
    public float damagePoint;

    [Header("컴포넌트")]
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Collider2D cd;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {

    }
}
