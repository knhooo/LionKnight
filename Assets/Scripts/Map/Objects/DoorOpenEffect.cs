
using UnityEngine;

public class DoorOpenEffect : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private void OnEnable()
    {
        GetComponent<FadeObject>().StartSpriteFade(lifeTime, 1f, 0f);
    }
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}