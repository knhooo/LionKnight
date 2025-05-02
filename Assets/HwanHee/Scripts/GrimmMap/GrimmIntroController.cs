using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GrimmIntroController : MonoBehaviour
{
    [Header("인트로 플레이")]
    public bool isIntroPlay = true;

    [Header("보스 프리팹")]
    [SerializeField] public GameObject bossGrimm;
    [SerializeField] public GameObject bossNightmareGrimm;

    [Header("보스 트리거")]
    [SerializeField] public Collider2D bossStartTrigger;
    [SerializeField] public FadeSprite fadeSprite;

    [Header("카메라")]
    [SerializeField] public GameObject cinemachineCamera;
    [SerializeField] public BoxCollider2D bossCameraBoundingShape;
    [SerializeField] public Transform[] CameraPos;
    [SerializeField] public float cameraMoveTime = 0.5f;

    [Space]
    [Header("BGM")]
    [SerializeField] public AudioSource heartBeatAudio;
    [SerializeField] public AudioClip bossBGM1;
    [SerializeField] public AudioClip bossBGM2;
    [SerializeField] public AudioClip bossBusrtAudio;
    [SerializeField] public AudioClip burstAudio1;
    [SerializeField] public AudioClip burstAudio2;
    [SerializeField] public AudioClip burstAudio3;
    [SerializeField] public AudioSource burstAudioLoop;

    [Space]
    [Header("효과1")]
    [SerializeField] public float effect1StartDelay = 1f;
    [SerializeField] public float effect1Duration = 0.5f;
    [SerializeField] public float grimmLight1FadeDuration = 0.1f;
    [SerializeField] public ParticleSystem particleSystemPlay;
    [SerializeField] public FadeInOutObject grimmIntroLight1;
    [SerializeField] public GrimmBackgroundScaleController[] backgroundScales;


    [Header("카메라 Shake")]
    [SerializeField] public float shakeAmplitude1 = 1.5f;
    [SerializeField] public float shakeFrequency1 = 15f;

    [Space]
    [Header("효과2")]
    [SerializeField] public float effect2StartDelay = 1f;
    [SerializeField] public float effect2Durtaion = 1f;
    [SerializeField] public FadeInOutObject grimmIntroLight2;
    [SerializeField] public float particleEmission = 800f;

    [Header("카메라 Shake")]
    [SerializeField] public float shakeAmplitude2 = 3f;
    [SerializeField] public float shakeFrequency2 = 20f;

    [Space]
    [Header("효과3")]
    [SerializeField] public float effect3StartDelay = 1.5f;
    [SerializeField] public float effect3Durtaion = 2f;
    [SerializeField] public GameObject FVX;

    [Header("카메라 Shake")]
    [SerializeField] public float shakeAmplitude3 = 4f;
    [SerializeField] public float shakeFrequency3 = 30f;

    [Header("GrimmShape")]
    [SerializeField] FadeInOutObject grimmSilhouette;
    [SerializeField] float grimmSilhouetteActiveTime = 1f;
    [SerializeField] float grimmSilhouetteGrowSpeed = 0.5f;
    [SerializeField] float grimmSilhouetteGrowSpeedBigger = 0.5f;

    [Space]
    [Header("마지막효과")]
    [Header("GrimmIntroUI")]
    [SerializeField] Canvas grimmTextCanvas;
    [SerializeField] public float finalEffectDurtaion = 1.7f;
    [SerializeField] public float moveToPlayerTime = 0.5f;
    [SerializeField] public float canvasFadeOutDuration = 0.5f;

    public Player player;
    public Vector3 destination;

    public bool isBossStart = false;

    GrimmIntroStep currentIntroStep;
    GrimmIntroStep1 grimmIntroStep1;
    GrimmIntroStep2 grimmIntroStep2;
    GrimmIntroStep3 grimmIntroStep3;

    private float bossGroundY;

    public void Awake()
    {
        //player = PlayerManager.instance.player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;

        grimmIntroStep1 = new GrimmIntroStep1(this);
        grimmIntroStep2 = new GrimmIntroStep2(this);
        grimmIntroStep3 = new GrimmIntroStep3(this);
        currentIntroStep = grimmIntroStep1;

    }

    public void Start()
    {
        TurnOffEffects();
        grimmSilhouette.gameObject.SetActive(false);
        fadeSprite.gameObject.SetActive(true);

        if (player.isInIntro)
        {
            fadeSprite.gameObject.SetActive(false);
        }
        BGMManager.instance.StopBGM();
    }

    public void TurnOffEffects()
    {
        grimmSilhouette.gameObject.SetActive(false);
        grimmIntroLight1.gameObject.SetActive(false);
        grimmIntroLight2.gameObject.SetActive(false);
        grimmTextCanvas.gameObject.SetActive(false);
        FVX.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == bossStartTrigger.transform && !player.isInIntro && isIntroPlay)
        {
            StartCoroutine(fadeSprite.StartFadeOut());

            isIntroPlay = false;
            bossGrimm.GetComponent<BossGrimm>().BossGrimmGreet();

            BGMManager.instance.PlayBGM();

            Invoke("DefualtValueSetting", 1f);
        }
    }

    private void DefualtValueSetting()
    {
        bossGroundY = bossGrimm.GetComponent<BossGrimm>().groundY;
    }

    public void NightmareGrimmTrigger()
    {
        player.isInIntro = true;
        destination = player.transform.position;

        bossStartTrigger.gameObject.SetActive(false);
        transform.SetParent(null);

        foreach (var backgroundScale in backgroundScales)
        {
            backgroundScale.StartPulsate();
        }

        Invoke("StartIntro", 2f);
    }

    public void StartIntro()
    {
        StartCoroutine(CameraMove());
        StartCoroutine(fadeSprite.StartFadeOut());

    }

    public IEnumerator CameraMove()
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
                BGMManager.instance.BGMFadeOut(0f);
                Invoke("PlayIntro", effect1StartDelay);
                break;
            }
        }
    }

    public void PlayIntro()
    {
        currentIntroStep.Enter();
    }

    public void ChangeStep(int stepIndex, float delayTime)
    {
        switch (stepIndex)
        {
            case 1:
                currentIntroStep = grimmIntroStep1;
                break;
            case 2:
                currentIntroStep = grimmIntroStep2;
                break;
            case 3:
                currentIntroStep = grimmIntroStep3;
                break;
        }

        StartCoroutine(StartNextStep(delayTime));
    }

    private IEnumerator StartNextStep(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        currentIntroStep.Enter();
    }

    public void ActiveGrimmSilhouette()
    {
        Invoke("GrimmSilhouetteSetScale", grimmSilhouetteActiveTime);
    }

    public void GrimmSilhouetteSetScale()
    {
        grimmSilhouette.gameObject.SetActive(true);
        StartCoroutine(SetGrimmSilhouetteScale());
    }

    public IEnumerator SetGrimmSilhouetteScale()
    {
        Vector3 originScale = grimmSilhouette.transform.localScale;

        float elapsed = 0f;
        while (elapsed <= grimmSilhouetteGrowSpeed)
        {
            elapsed += Time.deltaTime;
            grimmSilhouette.transform.localScale = Vector3.Lerp(originScale, new Vector3(0.7f, 0.7f, 0.7f), elapsed / grimmSilhouetteGrowSpeed);
            yield return null;
        }
        StartCoroutine(SetGrimmSilhouetteScaleBigger());
    }

    public IEnumerator SetGrimmSilhouetteScaleBigger()
    {
        Vector3 originScale = grimmSilhouette.transform.localScale;
        Vector3 originPos = grimmSilhouette.transform.position;

        float elapsed = 0f;
        while (elapsed <= grimmSilhouetteGrowSpeed)
        {
            elapsed += Time.deltaTime;
            grimmSilhouette.transform.localScale = Vector3.Lerp(originScale, new Vector3(60f, 60f, 60f), elapsed / grimmSilhouetteGrowSpeedBigger);
            grimmSilhouette.transform.position = Vector3.Lerp(originPos, originPos + new Vector3(0f, -13f, 0f), elapsed / grimmSilhouetteGrowSpeedBigger);
            yield return null;
        }
        ShowGrimmUI();
    }

    public void ShowGrimmUI()
    {
        SwitchConfiner();
        grimmTextCanvas.gameObject.SetActive(true);

        burstAudioLoop.Stop();

        SoundManager.Instance.audioSource.PlayOneShot(bossBusrtAudio);
        BGMManager.instance.SetNewBGM(bossBGM2);
        BGMManager.instance.PlayBGM();

        Invoke("EffectFinish", finalEffectDurtaion);
    }

    public void EffectFinish()
    {
        StartCoroutine(MoveToPlayer());
        grimmSilhouette.StartFadeInOut(canvasFadeOutDuration, 1f, 0f);
        TurnOffEffects();

        heartBeatAudio.gameObject.SetActive(false);
    }

    public IEnumerator MoveToPlayer()
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
        player.isInIntro = false;

        bossNightmareGrimm.SetActive(true);
        bossNightmareGrimm.GetComponent<BossGrimm>().BossGrimmNightmareStart(bossGroundY);
    }

    public void SwitchConfiner() => cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = bossCameraBoundingShape;
}
