using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UIdataManage : MonoBehaviour
{
    public static UIdataManage instance;

    public HUD curPlayer = new HUD();

    public string path;
    public int curSlot;

    private void Awake()
    {
        #region ΩÃ±€≈Ê
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Application.persistentDataPath + "/profile";
        print(path);
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(curPlayer);
        File.WriteAllText(path + curSlot.ToString(), data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + curSlot.ToString());
        curPlayer = JsonUtility.FromJson<HUD>(data);
    }

    public void DataClear()
    {
        curSlot = -1;
        curPlayer = new HUD();
    }
}