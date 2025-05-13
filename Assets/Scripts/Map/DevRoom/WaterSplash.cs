using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    [SerializeField] AudioClip waterSplashSound;

    private void Awake()
    {
        Player player = PlayerManager.instance.player;

        GetComponent<Rigidbody2D>().AddForce(new Vector2(player.facingDir * 2f, 0f), ForceMode2D.Impulse);

        if (player.facingDir == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        SoundManager.Instance.audioSource.PlayOneShot(waterSplashSound);
    }
}
