using UnityEngine;

public class Geo : AddForceObject
{
    [SerializeField] private AudioClip[] geoGrounds;
    [SerializeField] private AudioClip[] geoCollects;

    protected override void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        base.OnEnable();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            PlayerManager.instance.player.playerData.money++;
            gameObject.SetActive(false);
            Debug.Log("Money : " + PlayerManager.instance.player.playerData.money);

            int index = Random.Range(0, geoCollects.Length);
            SoundManager.Instance.audioSource.PlayOneShot(geoCollects[index]);
        }

        else if (collision.gameObject.GetComponent<BoxCollider2D>() != null)
        {
            int index = Random.Range(0, geoGrounds.Length);
            SoundManager.Instance.audioSource.PlayOneShot(geoGrounds[index]);
        }
    }
}
