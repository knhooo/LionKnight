using UnityEngine;

public class GeoEffect : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private float dir;

    public float SetDir(float _dir = -999f)
    {
        if (_dir != -999f)
            dir = _dir;
        else
            dir = Random.Range(0, 360);

        transform.rotation = Quaternion.Euler(0f, 0f, dir);
        return dir;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void AnimationTrigger()
    {
        PoolManager.instance.ReturnToPool(PoolType.GeoEffect, gameObject);
    }
}
