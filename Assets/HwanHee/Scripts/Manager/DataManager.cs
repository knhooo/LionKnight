using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    string path;
    string liftFileName = "lift_save.json";

    private void Awake()
    {
        #region 싱글톤
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        #endregion

        path = Application.dataPath + "/../Saves/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void SaveData()
    {
        SaveLift();
    }

    private void SaveLift()
    {
        Lift lift = GameObject.FindAnyObjectByType<Lift>();
        if (lift != null)
        {
            LiftData data = lift.liftData;
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Path.Combine(path, liftFileName), json);
        }
    }

    public void LoadData()
    {
        LoadLift();
    }

    private void LoadLift()
    {
        string fullPath = Path.Combine(path, liftFileName);

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
