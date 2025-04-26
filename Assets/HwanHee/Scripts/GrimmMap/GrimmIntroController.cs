using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GrimmIntroController : MonoBehaviour
{
    [Header("보스 트리거")]
    [SerializeField] private Collider2D bossStartTrigger;
    [SerializeField] private FadeSprite fadeSprite;

    [Header("카메라")]
    [SerializeField] private GameObject cameraConfiner;
    [SerializeField] private BoxCollider2D bossCameraBoundingShape;
    [SerializeField] private Transform[] CameraPos;
    [SerializeField] private float cameraMoveTime;

    [Header("카메라 Shake")]
    [SerializeField] private float shakeDelay = 2f;
    [SerializeField] private float shakeAmplitude = 0.5f; 
    [SerializeField] private float shakeFrequency = 8.0f; 
    [SerializeField] private float shakeDuration = 0.5f;  

    [Header("효과1")]
    [SerializeField] private float grimmLight1FadeDuration = 0.1f;
    [SerializeField] private ParticleSystem particleSystemToPlay;
    [SerializeField] private GrimmLight grimmLight1;

    Player player;

    private void Awake()
    {
        // player = PlayerManager.instance.player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == bossStartTrigger.transform)
        {
            transform.SetParent(null);
            StartCoroutine(CameraMove());
            StartCoroutine(fadeSprite.StartFadeOut());
        }
    }

    private IEnumerator CameraMove()
    {
        while (true)
        {
            for (int i = 0; i < CameraPos.Length; i++)
            {
                Vector3 startPos = transform.position;

                float elapsed = 0f;
                while (elapsed < cameraMoveTime)
                {
                    transform.position = Vector3.Lerp(startPos, CameraPos[i].transform.position, elapsed / cameraMoveTime);

                    elapsed += Time.deltaTime;

                    yield return null;
                }

                transform.position = CameraPos[i].transform.position;
            }

            if (transform.position == CameraPos[CameraPos.Length - 1].position)
            {
                Invoke("StartEffect1", shakeDelay);
                break;
            }
        }
    }

    private void StartEffect1()
    {
        cameraConfiner.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude, shakeFrequency, shakeDuration);
        particleSystemToPlay.Play();
        grimmLight1.StartFadeIn(grimmLight1FadeDuration);
    }

    private void SwitchConfiner() => cameraConfiner.GetComponent<CinemachineConfiner2D>().BoundingShape2D = bossCameraBoundingShape;
}
