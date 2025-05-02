using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class saveLoad : MonoBehaviour
{
    public static saveLoad instance;
    public saveData data = new saveData();
    
    public GameObject newgame;
    public GameObject loadprofile;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("InGame");
    }

    public void Save(int profileNumber)
    {
        string json = JsonUtility.ToJson(data);
        string path = GetProfilePath(profileNumber);
        File.WriteAllText(path, json);
    }

    public void Load(int profileNumber)
    {
        string path = GetProfilePath(profileNumber);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<saveData>(json);
        }
    }
    public void Delete()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);

            loadprofile.SetActive(false);
            newgame.SetActive(true);
        }
    }
    public bool SaveFileExists(int profileNumber)
    {
        string path = GetProfilePath(profileNumber);
        return File.Exists(path);
    }

    private string GetProfilePath(int profileNumber)
    {
        return Application.persistentDataPath + "/savefile_profile" + profileNumber + ".json";
    }

    internal bool SaveFileExists(List<int> profiles)
    {
        throw new NotImplementedException();
    }
}
