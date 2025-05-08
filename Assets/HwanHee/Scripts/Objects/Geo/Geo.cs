using UnityEngine;

public class Geo : MonoBehaviour
{
    [SerializeField] private AudioClip[] geoGrounds;
    [SerializeField] private AudioClip[] geoCollects;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            GeoEffect();

            PlayerManager.instance.player.playerData.money++;
            PoolManager.instance.ReturnToPool(PoolType.Geo, gameObject);

            Debug.Log("Money : " + PlayerManager.instance.player.playerData.money);

            int index = Random.Range(0, geoCollects.Length);
            SoundManager.Instance.audioSource.PlayOneShot(geoCollects[index]);
        }

        else if (collision.CompareTag("Ground"))
        {
            int index = Random.Range(0, geoGrounds.Length);
            SoundManager.Instance.audioSource.PlayOneShot(geoGrounds[index]);
        }
    }

    private void GeoEffect()
    {
        float dir = 0f;
        Vector3 playerPos = PlayerManager.instance.player.transform.position;
        for (int i = 0; i < 2; i++)
        {
            GameObject geoEffect = PoolManager.instance.Spawn(PoolType.GeoEffect, new Vector3(playerPos.x, playerPos.y + 0.4f), Quaternion.identity);

            if (i == 0)
                dir = geoEffect.GetComponent<GeoEffect>().SetDir();
            else
                geoEffect.GetComponent<GeoEffect>().SetDir(dir + 180f);
        }
    }
}
