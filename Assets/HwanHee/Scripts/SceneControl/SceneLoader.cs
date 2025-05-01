using UnityEngine;
using UnityEngine.Audio;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool isAutoLoad = true;
    [SerializeField] private string sceneToLoad;

    [Header("BGM")]
    [SerializeField] private float CurrentBGMVolume;
    [SerializeField] private float NewBGMVolume = 1f;
    [SerializeField] private AudioClip[] newAudioSources;

    private bool playerInTrigger;
    private bool isLoadScene = false;

    private void Update()
    {
        if (playerInTrigger && !isLoadScene)
        {
            if (isAutoLoad || (!isAutoLoad && Input.GetKeyDown(KeyCode.UpArrow)))
            {
                isLoadScene = true;

                if (newAudioSources.Length > 0)
                    BGMManager.instance.SetNewBGM(newAudioSources);
                BGMManager.instance.CurrentBGMVolume = CurrentBGMVolume;
                BGMManager.instance.NewBGMVolume = NewBGMVolume;

                SceneSaveLoadManager.instance.StartLoadScene(sceneToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
            playerInTrigger = false;
    }
}
