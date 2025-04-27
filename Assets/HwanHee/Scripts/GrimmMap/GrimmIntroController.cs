using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GrimmIntroController : MonoBehaviour
{
    [Header("보스 트리거")]
    [SerializeField] private Collider2D bossStartTrigger;
    [SerializeField] private FadeSprite fadeSprite;

    [Header("카메라")]
    [SerializeField] private GameObject cinemachineCamera;
    [SerializeField] private BoxCollider2D bossCameraBoundingShape;
    [SerializeField] private Transform[] CameraPos;
    [SerializeField] private float cameraMoveTime;

    [Space]
    [Header("BGM")]
    [SerializeField] private AudioSource heartBeatAudio;
    [SerializeField] private AudioSource bossBGM;
    [SerializeField] private BurstAudio burstAudio;
    [SerializeField] private AudioSource burstAudioLoop;

    [Space]
    [Header("효과1")]
    [SerializeField] private float effect1StartDelay = 2f;
    [SerializeField] private float effect1Duration = 0.5f;
    [SerializeField] private float grimmLight1FadeDuration = 0.1f;
    [SerializeField] private ParticleSystem particleSystemPlay;
    [SerializeField] private FadeInOutObject grimmIntroLight1;

    [Header("카메라 Shake")]
    [SerializeField] private float shakeAmplitude1 = 0.5f;
    [SerializeField] private float shakeFrequency1 = 8.0f;

    [Space]
    [Header("효과2")]
    [SerializeField] private float effectStartDelay2 = 1f;
    [SerializeField] private float effectDurtaion2 = 1f;
    [SerializeField] private FadeInOutObject grimmIntroLight2;
    [SerializeField] private float particleEmission = 800f;

    [Header("카메라 Shake")]
    [SerializeField] private float shakeAmplitude2 = 1.5f;
    [SerializeField] private float shakeFrequency2 = 15f;

    [Space]
    [Header("효과3")]
    [SerializeField] private float effectStartDelay3 = 1.5f;
    [SerializeField] private float effectDurtaion3 = 2f;
    [SerializeField] private GameObject FVX;

    [Header("카메라 Shake")]
    [SerializeField] private float shakeAmplitude3 = 3f;
    [SerializeField] private float shakeFrequency3 = 20f;

    [Header("GrimmShape")]
    [SerializeField] FadeInOutObject grimmShape;
    [SerializeField] float grimmShapeActiveTime = 1f;
    [SerializeField] float grimmShapeGrowSpeed = 1f;
    [SerializeField] float grimmShapeGrowSpeedBigger = 1f;

    [Space]
    [Header("마지막효과")]
    [Header("GrimmIntroUI")]
    [SerializeField] Canvas grimmTextCanvas;
    [SerializeField] private float finalEffectDurtaion = 1f;
    [SerializeField] private float moveToPlayerTime = 0.5f;
    [SerializeField] private float canvasFadeOutDuration = 0.5f;

    private Player player;
    private Vector3 destination;

    public bool isBossStart = false;
    public bool isInIntro = false;

    private void Awake()
    {
        //player = PlayerManager.instance.player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;
    }

    private void Start()
    {
        TurnOffEffects();
        grimmShape.gameObject.SetActive(false);
        fadeSprite.gameObject.SetActive(true);
    }

    private void TurnOffEffects()
    {
        grimmIntroLight1.gameObject.SetActive(false);
        grimmIntroLight2.gameObject.SetActive(false);
        grimmTextCanvas.gameObject.SetActive(false);
        FVX.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == bossStartTrigger.transform)
        {
            isInIntro = true;
            destination = player.transform.position;

            bossStartTrigger.gameObject.SetActive(false);
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
                Invoke("PlayEffect1", effect1StartDelay);
                break;
            }
        }
    }

    private void PlayEffect1()
    {
        cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude1, shakeFrequency1, effect1Duration);

        burstAudio.Play(0);

        particleSystemPlay.Play();
        grimmIntroLight1.gameObject.SetActive(true);
        grimmIntroLight1.StartFadeInOut(grimmLight1FadeDuration, 0f, 1f);

        Invoke("PlayEffect2", effectStartDelay2);
    }


    private void PlayEffect2()
    {
        cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude2, shakeFrequency2, effectDurtaion2);

        burstAudio.Play(1);

        particleSystemPlay.Stop();
        var emission = particleSystemPlay.emission;
        emission.rateOverTime = particleEmission;
        particleSystemPlay.Play();

        grimmIntroLight2.gameObject.SetActive(true);
        grimmIntroLight2.StartFadeInOut(grimmLight1FadeDuration, 0f, 1f);

        Invoke("PlayEffect3", effectStartDelay3);
    }

    private void PlayEffect3()
    {
        burstAudio.Play(2);
        burstAudioLoop.Play();

        cinemachineCamera.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude3, shakeFrequency3, effectDurtaion3);
        FVX.SetActive(true);
        Invoke("GrimmShapeSetScale", grimmShapeActiveTime);
    }

    private void GrimmShapeSetScale()
    {
        grimmShape.gameObject.SetActive(true);
        StartCoroutine(SetScale());
    }

    private IEnumerator SetScale()
    {
        Vector3 originScale = grimmShape.transform.localScale;

        float elapsed = 0f;
        while (elapsed <= grimmShapeGrowSpeed)
        {
            elapsed += Time.deltaTime;
            grimmShape.transform.localScale = Vector3.Lerp(originScale, new Vector3(0.7f, 0.7f, 0.7f), elapsed / grimmShapeGrowSpeed);
            yield return null;
        }
        StartCoroutine(SetScaleBigger());
    }

    private IEnumerator SetScaleBigger()
    {
        Vector3 originScale = grimmShape.transform.localScale;
        Vector3 originPos = grimmShape.transform.position;

        float elapsed = 0f;
        while (elapsed <= grimmShapeGrowSpeed)
        {
            elapsed += Time.deltaTime;
            grimmShape.transform.localScale = Vector3.Lerp(originScale, new Vector3(60f, 60f, 60f), elapsed / grimmShapeGrowSpeedBigger);
            grimmShape.transform.position = Vector3.Lerp(originPos, originPos + new Vector3(0f, -13f, 0f), elapsed / grimmShapeGrowSpeedBigger);
            yield return null;
        }
        ShowGrimmUI();
    }

    private void ShowGrimmUI()
    {
        SwitchConfiner();
        grimmTextCanvas.gameObject.SetActive(true);

        burstAudioLoop.Stop();
        bossBGM.Play();

        Invoke("EffectFinish", finalEffectDurtaion);
    }

    private void EffectFinish()
    {
        StartCoroutine(MoveToPlayer());
        grimmShape.StartFadeInOut(canvasFadeOutDuration, 1f, 0f);
        TurnOffEffects();

        heartBeatAudio.Stop();
    }

    private IEnumerator MoveToPlayer()
    {
        yield return new WaitForSeconds(canvasFadeOutDuration);

        float elapsed = 0f;
        while (elapsed <= moveToPlayerTime)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, destination, elapsed / moveToPlayerTime);
            yield return null;
        }

        cinemachineCamera.GetComponent<CinemachineCamera>().Follow = player.transform;
        cinemachineCamera.GetComponent<CinemachineCamera>().LookAt = player.transform;
        gameObject.SetActive(false);

        isBossStart = true;
        isInIntro = false;
    }

    private void SwitchConfiner() => cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = bossCameraBoundingShape;
}
