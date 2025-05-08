using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private GameObject triggerUI;
    [SerializeField] private Player player;

    private void Update()
    {
        if (player)
        {
            if (player.isDialog)
                triggerUI.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            player = collision.GetComponent<Player>();
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
