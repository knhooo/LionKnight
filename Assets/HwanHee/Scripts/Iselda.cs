using UnityEngine;

public class Iselda : MonoBehaviour
{
    enum Dir
    {
        Left = -1,
        Right = 1
    }

    enum State
    {
        Idle,
        Talk
    }

    [SerializeField] private Transform lookRightPos;
    [SerializeField] private AudioClip talkSound;

    private Animator anim;
    private Player player;

    private bool isPlayerCol = false;

    private Dir dir = Dir.Left;
    private State state = State.Idle;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        if (state != State.Talk && isPlayerCol && Input.GetKeyDown(KeyCode.UpArrow))
        {
            state = State.Talk;
            SoundManager.Instance.audioSource.PlayOneShot(talkSound);
            anim.SetBool("isTalk", true);
        }
        else if (state == State.Talk && Input.GetKeyDown(KeyCode.Escape))
        {
            state = State.Idle;
            anim.SetBool("isTalk", false);
        }

        if (dir != Dir.Right && state != State.Talk && player.transform.position.x > transform.position.x)
        {
            anim.SetBool("isRight", true);
            dir = Dir.Right;
        }
        else if (dir == Dir.Right && player.transform.position.x <= transform.position.x)
        {
            anim.SetBool("isRight", false);
            dir = Dir.Left;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerCol = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerCol = false;
        }
    }
}
