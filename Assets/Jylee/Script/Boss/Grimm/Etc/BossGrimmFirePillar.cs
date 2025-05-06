using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossGrimmFirePillar : MonoBehaviour
{
    public float activeDelay;
    public GameObject firePillarPrefab;
    public float activeDuration;
    public AudioClip pillarSound;

    private bool isActive;
    private GameObject pillar;

    private Collider2D cd;

    void Start()
    {
        cd = GetComponent<Collider2D>();
    }

    void Update()
    {
        activeDelay -= Time.deltaTime;

        if(activeDelay <= 0 && !isActive)
        {
            isActive = true;
            // 생성, 콜라이더 활성화
            pillar = Instantiate(firePillarPrefab, transform.position, Quaternion.identity);
            Destroy(pillar, 0.7f);
            cd.enabled = true;
            SoundManager.Instance.audioSource.PlayOneShot(pillarSound);
        }

        if (isActive)
        {
            activeDuration -= Time.deltaTime;
            if (activeDuration <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
