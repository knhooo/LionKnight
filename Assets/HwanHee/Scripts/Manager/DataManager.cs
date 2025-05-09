using UnityEngine;
using System.IO;

public class DataManager : Singleton<DataManager>
{
    string path;
    string liftSaveFileName = "lift_save.json";
    public string playerSaveFileName = "player_save.json";

    private Player player;
    private Lift lift;
    [HideInInspector] public string saveFiles = "test";
    public string saveFileName;

    public void RegisterPlayer(Player _player) => player = _player;
    public void RegisterLift(Lift _lift) => lift = _lift;

    protected override void Awake()
    {
        base.Awake();
        path = Path.Combine(Application.dataPath, "..", "Saves", saveFiles);
    }

    public void PrepareSaveDirectory()
    {
        path = Path.Combine(Application.dataPath, "..", "Saves", saveFiles);
        CreateFile();
    }

    private void CreateFile()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void SaveData()
    {
        CreateFile();
        SavePlayer();
        SaveLift();
    }

    private void SavePlayer()
    {
        if (player != null)
        {
            string json = JsonUtility.ToJson(player.GetSaveData());
            File.WriteAllText(Path.Combine(path, playerSaveFileName), json);
        }
    }

    private void SaveLift()
    {
        if (lift != null)
        {
            string json = JsonUtility.ToJson(lift.GetSaveData());
            File.WriteAllText(Path.Combine(path, liftSaveFileName), json);
        }
    }

    public void LoadData()
    {
        LoadPlayer();
        LoadLift();
    }

    public bool LoadPlayer()
    {
        if (player == null)
        {
            Debug.Log("플레이어 없음");
            return false;
        }

        string fullPath = Path.Combine(path, playerSaveFileName);

        if (File.Exists(fullPath))
        {
            string data = File.ReadAllText(fullPath);
            player.LoadFromData(JsonUtility.FromJson<PlayerData>(data));
            return false;
        }
        return true;
    }

    private void LoadLift()
    {
        if (lift == null)
        {
            Debug.Log("리프트 없음");
            return;
        }

        string fullPath = Path.Combine(path, liftSaveFileName);

        if (File.Exists(fullPath))
        {
            string data = File.ReadAllText(fullPath);
            lift.LoadFromData(JsonUtility.FromJson<LiftData>(data));
        }
    }
}
