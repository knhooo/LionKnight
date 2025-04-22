using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool isAutoLoad = true;
    [SerializeField] private GameObject sceneFaderPrefab;

    private SceneFader sceneFader;
    public string sceneToLoad;
    private bool playerInTrigger;

    private void Awake()
    {
        sceneFader = FindAnyObjectByType<SceneFader>();
        if (sceneFader == null)
            sceneFader = Instantiate(sceneFaderPrefab).GetComponent<SceneFader>();
    }

    private void Update()
    {
        if (playerInTrigger)
        {
            if (isAutoLoad || (!isAutoLoad && Input.GetKeyDown(KeyCode.UpArrow)))
            {
                sceneFader.FadeToScene(sceneToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
            playerInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
            playerInTrigger = false;
    }
}
