using UnityEngine;
using System.Collections;

public class GeoDeposit : ShakeObject
{
    SpriteRenderer sp;

    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject geoPrefab;
    [SerializeField] private AudioClip[] geoDepositHits;

    private int life = 4;

    private void Awake()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
    }

    public void HitGeoDeposit()
    {
        int index = Random.Range(0, geoDepositHits.Length);
        SoundManager.Instance.audioSource.PlayOneShot(geoDepositHits[index]);
        Shake(sp.transform, true);
        CreateGeo();
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
