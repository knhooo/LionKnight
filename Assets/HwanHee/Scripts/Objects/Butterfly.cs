using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [SerializeField] private Transform lightSource;
    [SerializeField] private float radius = 2f;

    private float directionChangeInterval;
    private float speed;

    private float angle;
    private float timer;
    private Vector3 targetPos;

    void Start()
    {
        angle = Random.Range(0f, 360f);
        PickNewTarget();

        speed = Random.Range(0.8f, 1f);
        directionChangeInterval = Random.Range(0.8f, 1.3f);
        timer = Random.Range(0f, 0.5f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= directionChangeInterval)
        {
            timer = 0f;
            PickNewTarget();
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);

    }

    void PickNewTarget()
    {
        angle += Random.Range(-90f, 90f);
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        targetPos = lightSource.position + offset;
    }
}
