using UnityEngine;

public class GrimmAudience : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("Audience", 0, Random.Range(0f, 1f));
            anim.speed = Random.Range(0.8f, 1f);
        }
    }
}
