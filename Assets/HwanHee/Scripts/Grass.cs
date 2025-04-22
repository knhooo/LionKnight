using UnityEngine;

public class Grass : MonoBehaviour
{
    SpriteRenderer sp;
    Animator anim;

    [SerializeField] string AnimationName;
    [SerializeField] bool canBeCut = false;
    [SerializeField] Sprite cutImg;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play(AnimationName, 0, Random.Range(0f, 1f));
            anim.speed = Random.Range(0.8f, 1.3f);
            Debug.Log("");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canBeCut)
            CutGraas();
    }

    void CutGraas()
    {
        sp.sprite = cutImg;
        for (int i = 0; i < 25; i++)
        {
            int num = Random.Range(0, 3);

            CutGrassParticle particle = GameManager.instance.poolManager.Get(num).GetComponent<CutGrassParticle>();
            if (particle != null)
            {
                particle.SetPosition(transform.position);
            }
        }
    }
}
