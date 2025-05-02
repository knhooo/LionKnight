using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Audio;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool isAutoLoad = true;
    [SerializeField] private bool isBell = false;
    [SerializeField] private string sceneToLoad;

    [Header("BGM")]
    [SerializeField] private float currentBGMVolume;
    [SerializeField] private float newBGMVolume = 1f;
    [SerializeField] private AudioClip[] newAudioSources;
    [SerializeField] private AudioClip bell;
    [SerializeField] private bool playImmediately = true;

    private bool playerInTrigger;
    private bool isLoadScene = false;

    private void Update()
    {
        if (playerInTrigger && !isLoadScene)
        {
            if (isAutoLoad || (!isAutoLoad && Input.GetKeyDown(KeyCode.UpArrow)))
            {
                isLoadScene = true;

                if(isBell)
                    SoundManager.Instance.audioSource.PlayOneShot(bell);

                if (newAudioSources.Length > 0)
                    BGMManager.instance.SetNewBGMs(newAudioSources);
                BGMManager.instance.SetValues(currentBGMVolume, newBGMVolume, playImmediately);

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
