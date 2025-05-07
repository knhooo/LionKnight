using System.Collections;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private SpriteRenderer sp;
    private Animator anim;

    [Header("Cut")]
    [SerializeField] private bool canBeCut = false;
    [SerializeField] private Sprite cutImg;
    [SerializeField] private GameObject grassParticle;
    [SerializeField] private AudioClip grassCut;
    [Header("Move")]
    [SerializeField] private AudioClip[] grassMoveAudio;

    private bool isCut = false;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void CutGraas()
    {
        if (canBeCut == false)
            return;
        canBeCut = false;

        anim.enabled = false;
        sp.sprite = cutImg;

        SoundManager.Instance.audioSource.PlayOneShot(grassCut);

        for (int i = 0; i < 25; i++)
        {
            isCut = true;
            PoolManager.instance.Spawn(PoolType.GrassParticle, grassParticle, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCut)
        {
            anim.SetTrigger("Move");
            int index = Random.Range(0, grassMoveAudio.Length);
            SoundManager.Instance.audioSource.PlayOneShot(grassMoveAudio[index], 5f);
        }
    }
}
