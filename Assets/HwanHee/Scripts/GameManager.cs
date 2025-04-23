using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int a = 0;
        }
    }
}