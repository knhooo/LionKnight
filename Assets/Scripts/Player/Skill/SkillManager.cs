using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dash { get; private set; }
    public Focus_Skill focus { get; private set; }
    public Spirit_Skill spirit { get; private set; }
    public Howling_Skill howling { get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        focus = GetComponent<Focus_Skill>();
        spirit = GetComponent<Spirit_Skill>();
        howling = GetComponent<Howling_Skill>();
    }
}
