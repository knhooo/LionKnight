using UnityEngine;

public class Geo : MonoBehaviour
{
    [SerializeField] private AudioClip[] geoGrounds;
    [SerializeField] private AudioClip[] geoCollects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.player.playerData.money++;
            transform.parent.gameObject.SetActive(false);
            Debug.Log("Money : " + PlayerManager.instance.player.playerData.money);

            int index = Random.Range(0, geoCollects.Length);
            SoundManager.Instance.audioSource.PlayOneShot(geoCollects[index]);
        }

        else if(collision.CompareTag("Ground"))
        {
            int index = Random.Range(0, geoGrounds.Length);
            SoundManager.Instance.audioSource.PlayOneShot(geoGrounds[index]);
        }
    }
}
