using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager poolManager;
    public Player player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
        DontDestroyOnLoad(this);

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
}