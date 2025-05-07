
using UnityEngine;

public class WaterParticle : MonoBehaviour
{
    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;
    [SerializeField] private float startScaleMin;
    [SerializeField] private float startScaleMax;
    [SerializeField] private float endScaleMin;
    [SerializeField] private float endScaleMax;
    [SerializeField] private float lifeTimeMin;
    [SerializeField] private float lifeTimeMax;

    private float speed;
    private float startScale;
    private float endScale;
    private float lifeTime;

    private float elapsed = 0f;

    private bool isFadeOut = false;

    private void Awake()
    {
        speed = Random.Range(speedMin, speedMax);
        startScale = Random.Range(startScaleMin, startScaleMax);
        endScale = Random.Range(endScaleMin, endScaleMax);
        lifeTime = Random.Range(lifeTimeMin, lifeTimeMax);

        transform.localScale = new Vector3(startScale, startScale, startScale);
    }

    private void OnEnable()
    {
        isFadeOut = false;
        elapsed = 0f;

        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    private void Update()
    {
        CheckAndFadeOut();

        UpdateScaleAndPosition();
    }

    private void CheckAndFadeOut()
    {
        elapsed += Time.deltaTime;

        if (elapsed > lifeTime && !isFadeOut)
        {
            isFadeOut = true;
            GetComponent<FadeObject>().StartSpriteFade(0.5f, 1f, 0f);
        }

        else if (isFadeOut && GetComponent<SpriteRenderer>().color.a == 0f)
        {
            PoolManager.instance.ReturnToPool(PoolType.WaterParticle, gameObject);
            return;
        }
    }

    private void UpdateScaleAndPosition()
    {
        float scale = Mathf.Lerp(this.startScale, endScale, elapsed / lifeTime);
        transform.localScale = new Vector3(scale, scale, scale);

        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}
