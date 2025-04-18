using UnityEngine;

public class Grass : MonoBehaviour
{
    Animator anim;
    [SerializeField] string AnimationName;
    [SerializeField] bool canBeCut = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.Play(AnimationName, 0, Random.Range(0f, 1f));
        anim.speed = Random.Range(0.8f, 1.3f);
    }

    private void Update()
    {
        if (canBeCut) 
            CutGraas();
    }

    void CutGraas()
    {

    }
}
