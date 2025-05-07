using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Geo,
    GeoEffect,
    GrassParticle,
    BrokenParticle,
    GrimmParticle
}

[System.Serializable]
public class Pool
{
    public PoolType type;
    public GameObject prefab;
    public int size;
}

public class PoolManager : Singleton<PoolManager>
{
    public List<Pool> pools;
    private Dictionary<PoolType, Queue<GameObject>> poolDict;
    private Dictionary<PoolType, GameObject> poolPrefab;

    protected override void Awake()
    {
        base.Awake();

        poolDict = new Dictionary<PoolType, Queue<GameObject>>();
        poolPrefab = new Dictionary<PoolType, GameObject>();

        Create();
    }

    private void Create()
    {
        foreach (Pool pool in pools)
        {
            if (pool.type == PoolType.GeoEffect)
            {
                int a = 0;
            }
            Queue<GameObject> queue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            poolDict[pool.type] = queue;
            poolPrefab[pool.type] = pool.prefab;
        }
    }

    public GameObject Spawn(PoolType type, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(type))
        {
            Debug.LogError("풀매니저에 " + tag + "옵젝 없음");
        }

        Queue<GameObject> pool = poolDict[type];
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(poolPrefab[type], transform);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(PoolType type, GameObject obj)
    {
        if (!poolDict.ContainsKey(type))
        {
            Debug.Log("풀매니저에 " + tag + "옵젝 없음");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDict[type].Enqueue(obj);
    }
}
