using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private GameObject triggerUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            triggerUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            triggerUI.SetActive(false);
        }
    }
}
