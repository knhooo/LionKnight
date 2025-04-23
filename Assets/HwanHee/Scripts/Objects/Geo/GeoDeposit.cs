using Mono.Cecil;
using UnityEngine;
using System.Collections;

public class GeoDeposit : MonoBehaviour
{
    private SpriteRenderer sp;

    [SerializeField] private Sprite sprite;

    [SerializeField] private float amplitude = 0.1f; // 흔들림 크기
    [SerializeField] private float frequency = 1f; // 흔들림 속도
    [SerializeField] private float shakeDuration = 0.5f;

    [SerializeField] private GameObject geoPrefab;

    private Vector3 startPos;

    private int life = 4;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        startPos = transform.position;
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
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float zOffset = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = startPos + new Vector3(0f, 0f, zOffset);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
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
