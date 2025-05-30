using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool isAutoLoad = true;
    [SerializeField] private bool isBell = false;
    [SerializeField] private bool isStagStation = false;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private Stag stag;

    [Header("BGM")]
    [SerializeField] private float currentBGMTargetVolume;
    [SerializeField] private float newBGMTargetVolume = 1f;
    [SerializeField] private AudioClip[] newAudioClips;
    [SerializeField] private AudioClip bell;

    [SerializeField] private GameObject triggerUI;

    private bool playerInTrigger;
    private bool isLoadScene = false;

    private void OnValidate()
    {
        if (newAudioClips == null || newAudioClips.Length == 0)
        {
            newBGMTargetVolume = 0;
        }

        if (!isBell)
        {
            bell = null;
        }
    }

    private void Update()
    {
        if (playerInTrigger && !isLoadScene)
        {
            if (isAutoLoad || (!isAutoLoad && Input.GetKeyDown(KeyCode.UpArrow)))
            {
                isLoadScene = true;
                if (isBell)
                    SoundManager.Instance.audioSource.PlayOneShot(bell);

                if (!isStagStation)
                    StartLoadScene();
                else
                    stag.StartStag();
            }
        }

        if (playerInTrigger)
        {
            triggerUI.SetActive(true);
        }
        else
        {
            triggerUI.SetActive(false);
        }
    }

    public void StartLoadScene()
    {
        BGMManager.instance.SetBGM(newAudioClips, currentBGMTargetVolume, newBGMTargetVolume);

        PlayerManager.instance.player.playerData.toSceneName = sceneToLoad;

        SceneSaveLoadManager.instance.StartLoadScene(sceneToLoad);
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
        {
            playerInTrigger = false;
        }
    }
}
