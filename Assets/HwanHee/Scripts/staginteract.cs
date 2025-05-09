using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class MapData
{
    public string mapName;
    public string sceneName;
}

public class staginteract : MonoBehaviour
{
    public MapData[] maps;
    public GameObject buttonPrefab;
    public Transform scrollContent;
    public VideoPlayer stagTunnel;

    private void Start()
    {
        
        foreach (var map in maps)
        {
            CreateButton(map);
        }

        stagTunnel.gameObject.SetActive(false);
        stagTunnel.loopPointReached += OnVideoEnd;
    }

    private void CreateButton(MapData map)
    {

        GameObject buttonObj = Instantiate(buttonPrefab, scrollContent);
        Button button = buttonObj.GetComponent<Button>();
        Text buttonText = buttonObj.GetComponentInChildren<Text>();

        buttonText.text = map.mapName;

        button.onClick.AddListener(() => OnMapButtonClicked(map));
    }

    private void OnMapButtonClicked(MapData map)
    {
        stagTunnel.gameObject.SetActive(true);
        stagTunnel.Play();

        StartCoroutine(LoadSceneAfterVideo(map.sceneName));
    }

    private IEnumerator LoadSceneAfterVideo(string sceneName)
    {
        while (stagTunnel.isPlaying)
        {
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        stagTunnel.gameObject.SetActive(false);
    }
}