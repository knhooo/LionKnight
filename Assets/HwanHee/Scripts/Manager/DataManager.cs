using UnityEngine;
using System.IO;

public class DataManager : Singleton<DataManager>
{
    string path;
    string liftSaveFileName = "lift_save.json";
    string playerSaveFileName = "player_save.json";

    private Player player;
    private Lift lift;

    public void RegisterPlayer(Player _player) => player = _player;
    public void RegisterLift(Lift _lift) => lift = _lift;

    protected override void Awake()
    {
        base.Awake();
        PrepareSaveDirectory();
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

    private void LoadPlayer()
    {
        if (player == null)
        {
            Debug.Log("플레이어 없음");
            return;
        }

        string fullPath = Path.Combine(path, playerSaveFileName);

        if (File.Exists(fullPath)) 
        {
            string data = File.ReadAllText(fullPath);
            player.LoadFromData(JsonUtility.FromJson<PlayerData>(data));
        }
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
