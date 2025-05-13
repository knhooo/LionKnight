using UnityEngine;
using System.IO;

public class DataManager : Singleton<DataManager>
{
    public string path;
    public string playerSaveFileName = "player_save.json";
    public string shopSaveFileName = "shop_save.json";
    public string bossDeadSaveFileName = "bossDeath_save.json";
    private string liftSaveFileName = "lift_save.json";

    public ShopData shopData = new ShopData();
    public BossDeadData bossDeadData = new BossDeadData();

    private Player player;
    private Lift lift;
    [HideInInspector] public string saveFiles = "test";

    public void RegisterPlayer(Player _player) => player = _player;
    public void RegisterLift(Lift _lift) => lift = _lift;

    public void ChangeSaveFileName(string _saveFileName) => path = Path.Combine(Application.dataPath, "..", "Saves", _saveFileName);

    protected override void Awake()
    {
        base.Awake();

        path = Path.Combine(Application.dataPath, "..", "Saves", saveFiles);
        CreateFile();
    }

    public void CreateFile()
    {
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
        string json = string.Empty;

        if (player != null)
        {
            json = JsonUtility.ToJson(player.GetSaveData());
        }
        else
        {
            PlayerData playerData = new PlayerData();
            json = JsonUtility.ToJson(playerData);
        }

        File.WriteAllText(Path.Combine(path, playerSaveFileName), json);
    }

    public void SaveShop()
    {
        string json = string.Empty;
        json = JsonUtility.ToJson(shopData);
        File.WriteAllText(Path.Combine(path, shopSaveFileName), json);
    }

    public void SaveBossDeath()
    {
        bossDeadData.isDead = true;

        string json = string.Empty;
        json = JsonUtility.ToJson(bossDeadData);
        File.WriteAllText(Path.Combine(path, bossDeadSaveFileName), json);
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
        LoadBossDead();
    }

    private bool LoadPlayer()
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

    private void LoadBossDead()
    {
        string fullPath = Path.Combine(path, bossDeadSaveFileName);

        if (File.Exists(fullPath))
        {
            string data = File.ReadAllText(fullPath);
            bossDeadData = JsonUtility.FromJson<BossDeadData>(data);
        }
    }

    private void LoadLift()
    {
        if (lift == null)
            return;

        string fullPath = Path.Combine(path, liftSaveFileName);

        if (File.Exists(fullPath))
        {
            string data = File.ReadAllText(fullPath);
            lift.LoadFromData(JsonUtility.FromJson<LiftData>(data));
        }
    }
}
