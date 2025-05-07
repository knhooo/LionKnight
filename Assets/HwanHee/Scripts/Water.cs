using System.Collections;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float xPosMin;
    [SerializeField] private float xPosMax;

    [Header("Prefab")]
    [SerializeField] private GameObject waterLand;
    [SerializeField] private GameObject waterSplashUp;
    [SerializeField] private GameObject waterSplash;

    [Header("Audio")]
    [SerializeField] AudioClip waterLandAudio;
    [SerializeField] AudioClip waterSplashUpAudio;
    [SerializeField] AudioClip waterSplashAudio;

    [Space]
    [SerializeField] private float waterParticleSpawnDelay;

    private Player player;
    private Coroutine particleCoroutine;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.audioSource.PlayOneShot(waterLandAudio);
            particleCoroutine = StartCoroutine(SpawnParticles());
            Instantiate(waterLand, new Vector3(player.transform.position.x, player.transform.position.y + 0.3f), Quaternion.identity);
        }
    }

    private IEnumerator SpawnParticles()
    {
        while (true)
        {
            Vector2 pos = player.transform.position;
            pos.x += Random.Range(xPosMin, xPosMax);
            pos.y += 0.3f;

            PoolManager.instance.Spawn(PoolType.WaterParticle, pos, Quaternion.identity);

            yield return new WaitForSeconds(waterParticleSpawnDelay);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.audioSource.PlayOneShot(waterLandAudio);
            StopCoroutine(particleCoroutine);
            StartWaterSplashUpAnim();
        }
    }

    private void StartWaterSplashUpAnim()
    {
        GameObject waterSplash = Instantiate(waterSplashUp, new Vector3(player.transform.position.x, player.transform.position.y + 0.3f), Quaternion.identity);

        int index = Random.Range(0, 2);
        waterSplash.GetComponent<Animator>().SetInteger("index", index);
    }
}
