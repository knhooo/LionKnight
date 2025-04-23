using UnityEngine;

public class Grass : MonoBehaviour
{
    private SpriteRenderer sp;
    private Animator anim;

    [SerializeField] private string AnimationName;
    [SerializeField] private bool canBeCut = false;
    [SerializeField] private Sprite cutImg;
    [SerializeField] private GameObject grassParticle;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play(AnimationName, 0, Random.Range(0f, 1f));
            anim.speed = Random.Range(0.8f, 1.3f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && canBeCut)
        {
            CutGraas();
        }
    }

    void CutGraas()
    {
        sp.sprite = cutImg;
        canBeCut = false;
        for (int i = 0; i < 25; i++)
        {
            PoolManager.instance.Spawn(PoolType.GrassParticle, grassParticle, transform.position, Quaternion.identity);
        }
    }
}
