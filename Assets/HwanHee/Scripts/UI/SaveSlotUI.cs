using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    [SerializeField] private GameObject saveFileUI;
    [SerializeField] private GameObject geoCountUI;
    [SerializeField] private GameObject emptyFileUI;

    [Space]
    [SerializeField] private string saveFileName = "SaveFile1";
    [Space]
    [SerializeField] private AudioClip[] dirtmouthAudioclips;

    private string path;

    private void Awake()
    {
        path = Path.Combine(Application.dataPath, "..", "Saves", saveFileName);

        if (Directory.Exists(path))
            SaveFileExist();
        else
            SaveFileNoExist();
    }

    public void SaveSlotInitialize()
    {
        if (Directory.Exists(path))
        {
            SaveFileExist();

            string pullPath = Path.Combine(path, DataManager.instance.playerSaveFileName);
            if (File.Exists(pullPath))
            {
                string data = File.ReadAllText(pullPath);

                if (string.IsNullOrWhiteSpace(data))
                {
                    geoCountUI.GetComponent<Text>().text = "0";
                }
                else
                {
                    int money = JsonUtility.FromJson<PlayerData>(data).money;
                    geoCountUI.GetComponent<Text>().text = (money > 0) ? money.ToString() : "0";
                }
            }
            else
            {
                geoCountUI.GetComponent<Text>().text = "0";
            }
        }
        else
            SaveFileNoExist();
    }

    private void SaveFileExist()
    {
        emptyFileUI.SetActive(false);
        saveFileUI.SetActive(true);
    }

    private void SaveFileNoExist()
    {
        emptyFileUI.SetActive(true);
        saveFileUI.SetActive(false);
    }

    public void LoadScene()
    {
        DataManager.instance.saveFiles = saveFileName;
        SceneSaveLoadManager.instance.StartLoadScene("Dirtmouth", true);
        BGMManager.instance.SetBGM(dirtmouthAudioclips, 0f, 1f);
    }

    public void ClearSave()
    {
        if (Directory.Exists(path))
        {
            try
            {
                Directory.Delete(path, true);
                Debug.Log($"폴더 삭제됨: {path}");

                ChangeSaveSlotUI();
            }
            catch (IOException ex)
            {
                Debug.LogError($"폴더 삭제 실패: {ex.Message}");
            }
        }
    }

    private void ChangeSaveSlotUI()
    {
        saveFileUI.GetComponent<FadeUI>().FadeInOut(1f, 0f);
        emptyFileUI.SetActive(true);
        emptyFileUI.GetComponent<FadeUI>().FadeInOut(0f, 1f, 0.7f);
    }
}
