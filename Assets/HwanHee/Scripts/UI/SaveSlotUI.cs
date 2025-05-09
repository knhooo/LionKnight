using UnityEngine;

public class SaveSlotUI : MonoBehaviour
{
    [SerializeField] private GameObject[] SaveUI;
    [SerializeField] private GameObject EmptyUI;
    [Space]
    [SerializeField] private Canvas canvas;
    [SerializeField] private string saveFileName = "SaveFile1";
    [Space]
    [SerializeField] private AudioClip[] dirtmouthAudioclips;


    private void Awake()
    {
        foreach (GameObject saveUI in SaveUI)
        {
            saveUI.SetActive(false);
        }

        EmptyUI.SetActive(true);
    }

    public void LoadScene()
    {
        DataManager.instance.saveFiles = saveFileName;
        SceneSaveLoadManager.instance.StartLoadScene("Dirtmouth", true);
        BGMManager.instance.SetBGM(dirtmouthAudioclips, 0f, 1f);
    }
}
