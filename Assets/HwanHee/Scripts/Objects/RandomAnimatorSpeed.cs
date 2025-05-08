using UnityEngine;

public class RandomAnimatorSpeed : MonoBehaviour
{
    [SerializeField] private float minSpeed = 0.8f;
    [SerializeField] private float maxSpeed = 1;

    [SerializeField] private string animName;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play(animName, 0, Random.Range(0f, 1f));
            anim.speed = Random.Range(minSpeed, 1f);
        }
    }
}
