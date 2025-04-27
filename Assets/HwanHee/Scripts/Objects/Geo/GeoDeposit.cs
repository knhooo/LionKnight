using UnityEngine;
using System.Collections;

public class GeoDeposit : MonoBehaviour
{
    private SpriteRenderer sp;

    [SerializeField] private Sprite sprite;

    [SerializeField] private float shakeAmount = 0.1f;
    [SerializeField] private float shakeFrequency = 10f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private GameObject geoPrefab;

    private float shakeTimer;

    private Vector3 originalPos;
    private bool isShaking = false;

    private int life = 4;

    private void Awake()
    {
        shakeTimer = shakeDuration;
        sp = GetComponentInChildren<SpriteRenderer>();
        originalPos = transform.position;
    }

    private void Update()
    {
        if (isShaking)
        {
            shakeTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && life > 0)
        {
            StartCoroutine(ShakeCoroutine());
            CreateGeo();
        }
    }


    private IEnumerator ShakeCoroutine()
    {
        if (isShaking)
            yield break;
        isShaking = true;

        float elapsed = 0f;
        while (shakeTimer >= 0)
        {
            elapsed += Time.deltaTime;

            // 주파수 조절
            float offsetX = Mathf.Sin(elapsed * Mathf.PI * shakeFrequency) * shakeAmount; 
            sp.transform.position = originalPos + new Vector3(offsetX, 0f, 0f);
            yield return null;
        }

        isShaking = false;
        shakeTimer = shakeDuration;
        sp.transform.position = originalPos;
    }

    private void CreateGeo()
    {
        if (life == 1)
            sp.sprite = sprite;

        int geoCount = 0;

        if (life == 1)
            geoCount = 9;
        else
            geoCount = 3;

        for (int i = 0; i < geoCount; i++)
        {
            Geo geo = PoolManager.instance.Spawn(PoolType.Geo, geoPrefab, transform.position, Quaternion.identity).GetComponent<Geo>();
        }

        life--;
    }
}
