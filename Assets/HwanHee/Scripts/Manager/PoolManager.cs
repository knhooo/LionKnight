using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Geo,
    GrassParticle,
    BrokenParticle
}

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<PoolType, Queue<GameObject>> poolDict;

    protected override void Awake()
    {
        base.Awake();
        poolDict = new Dictionary<PoolType, Queue<GameObject>>();
    }

    private void Create(PoolType type, GameObject prefab)
    {
        Queue<GameObject> objPool = new Queue<GameObject>();

        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        objPool.Enqueue(obj);

        poolDict.Add(type, objPool);
    }

    public GameObject Spawn(PoolType type, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(type))
            Create(type, prefab);

        else if (poolDict[type].Count < 1)
        {
            GameObject newObj = Instantiate(prefab, transform);
            newObj.SetActive(false);
            poolDict[type].Enqueue(newObj);
        }

        Queue<GameObject> pool = poolDict[type];
        GameObject obj = pool.Dequeue();

        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(position, rotation);

        return obj;
    }

    public void ReturnToPool(PoolType type, GameObject obj)
    {
        if (!poolDict.ContainsKey(type))
        {
            Debug.Log("풀매니저에 " + tag + "옵젝 없음");
            return;
        }

        obj.SetActive(false);
        poolDict[type].Enqueue(obj);
    }
}