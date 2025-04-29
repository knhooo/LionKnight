using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool isAutoLoad = true;
    [SerializeField] private string sceneToLoad;

    private bool playerInTrigger;

    private void Update()
    {
        if (playerInTrigger)
        {
            if (isAutoLoad || (!isAutoLoad && Input.GetKeyDown(KeyCode.UpArrow)))
            {
                SceneSaveLoadManager.instance.StartLoadScene(sceneToLoad);
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
