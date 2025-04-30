using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    string path;
    string liftSaveFileName = "lift_save.json";
    string playerSaveFileName = "player_save.json";

    private void Awake()
    {
        Singleton();

        PrepareSaveDirectory();
    }

    private void Singleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void PrepareSaveDirectory()
    {
        path = Application.dataPath + "/../Saves/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void SaveData()
    {
        SavePlayer();
        SaveLift();
    }

    private void SavePlayer()
    {
        //Player player = PlayerManager.instance.player;
        Player player = GameObject.FindAnyObjectByType<Player>();

        if (player != null)
        {
            PlayerData data = player.playerData;
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Path.Combine(path, playerSaveFileName), json);
        }
    }

    private void SaveLift()
    {
        Lift lift = GameObject.FindAnyObjectByType<Lift>();
        if (lift != null)
        {
            LiftData data = lift.liftData;
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Path.Combine(path, liftSaveFileName), json);
        }
    }

    public void LoadData()
    {
        LoadPlayer();
        LoadLift();
    }

    private void LoadPlayer()
    {
        string fullPath = Path.Combine(path, playerSaveFileName);

        if (!File.Exists(fullPath))
            return;

        //Player player = PlayerManager.instance.player;
        Player player = GameObject.FindAnyObjectByType<Player>();

        if (player != null)
        {
            string data = File.ReadAllText(fullPath);
            player.playerData = JsonUtility.FromJson<PlayerData>(data);
        }
    }

    private void LoadLift()
    {
        string fullPath = Path.Combine(path, liftSaveFileName);

        if (!File.Exists(fullPath))
            return;

        Lift lift = GameObject.FindAnyObjectByType<Lift>();
        if (lift != null)
        {
            string data = File.ReadAllText(fullPath);
            lift.liftData = JsonUtility.FromJson<LiftData>(data);
        }
    }
}
