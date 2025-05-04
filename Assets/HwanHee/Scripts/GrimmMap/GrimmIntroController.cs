using System.Collections;
using System.Diagnostics;
using Unity.Cinemachine;
using UnityEngine;

public enum CinematicStep
{
    Intro1, Intro2, FinalIntro, HalfHP, Outro1, Outro2, FinalOutro
}

public class GrimmIntroController : MonoBehaviour
{
    [Header("인트로 플레이")]
    public bool isIntroPlay = true;

    [Header("보스 프리팹")]
    [SerializeField] public GameObject bossGrimm;
    [SerializeField] public GameObject bossNightmareGrimm;

    [Header("보스 트리거")]
    [SerializeField] public Collider2D bossStartTrigger;
    [SerializeField] public FadeObject fadeSprite;
    [SerializeField] public float fadeSpriteDurtaion;

    [Header("카메라")]
    [SerializeField] public GameObject cinemachineCamera;
    [SerializeField] public BoxCollider2D bossCameraBoundingShape;
    [SerializeField] public Transform[] CameraPos;
    [SerializeField] public float cameraMoveTime = 0.5f;

    [Space]
    [Header("BGM")]
    [SerializeField] public AudioClip[] bossBGM1;
    [SerializeField] public AudioClip[] bossBGM2;
    [SerializeField] public AudioClip bossBusrtAudio;
    [SerializeField] public AudioClip burstAudio1;
    [SerializeField] public AudioClip burstAudio2;
    [SerializeField] public AudioClip burstAudio3;
    [SerializeField] public AudioSource burstAudioLoop;

    [Space]
    [Header("GrimmIntroUI")]
    [Header("효과1")]
    [SerializeField] public float intro1StartDelay = 1f;
    [SerializeField] public float fadeIntroEffect1Duration = 0.1f;
    [SerializeField] public FadeObject grimmIntroLightSP1;
    [SerializeField] public FadeObject grimmIntroLight1;
    [SerializeField] public ParticleSystem particleEffect;
    [SerializeField] public GrimmPulsate[] grimmPulsates;
    [SerializeField] public float pulseValue;
    [SerializeField] public float pulseDelayTime;


    [Header("카메라 Shake")]
    [SerializeField] public float introCameraShake1Duration = 0.5f;
    [SerializeField] public float shakeAmplitude1 = 1.5f;
    [SerializeField] public float shakeFrequency1 = 15f;

    [Space]
    [Header("효과2")]
    [SerializeField] public float intro2StartDelay = 1f;
    [SerializeField] public float fadeIntroEffect2Duration = 1f;
    [SerializeField] public FadeObject grimmIntroLightSP2;
    [SerializeField] public FadeObject grimmIntroLight2;
    [SerializeField] public float particleEmission = 800f;

    [Header("카메라 Shake")]
    [SerializeField] public float introCameraShake2Duration = 1f;
    [SerializeField] public float shakeAmplitude2 = 3f;
    [SerializeField] public float shakeFrequency2 = 20f;

    [Space]
    [Header("효과3")]
    [SerializeField] public float intro3StartDelay = 1.5f;
    [SerializeField] public GameObject FVX;

    [Header("카메라 Shake")]
    [SerializeField] public float introCameraShake3Duration = 2f;
    [SerializeField] public float shakeAmplitude3 = 4f;
    [SerializeField] public float shakeFrequency3 = 30f;

    [Header("GrimmShape")]
    [SerializeField] FadeObject grimmSilhouette;
    [SerializeField] float grimmSilhouetteActiveTime = 1f;
    [SerializeField] float grimmSilhouetteGrowSpeed = 0.5f;
    [SerializeField] float grimmSilhouetteGrowSpeedBigger = 0.5f;

    [Space]
    [Header("마지막효과")]
    [SerializeField] Canvas grimmTextCanvas;
    [SerializeField] public float finalIntroEffectDurtaion = 1.7f;
    [SerializeField] public float moveToPlayerTime = 0.5f;
    [SerializeField] public float canvasFadeOutDuration = 0.5f;

    [Space]
    [Header("보스 체력 절반 이하")]
    [SerializeField] public FadeObject[] eyes;
    [SerializeField] public float fadeEyesDuration;
    [SerializeField] public float newPulseValue;
    [SerializeField] public float newPulseDalyTime;

    [Space]
    [Header("아웃트로1")]


    public Player player;
    public Vector3 destination;

    public bool isBossStart = false;

    GrimmCinematicStep currentIntroStep;
    GrimmIntroStep1 grimmIntroStep1;
    GrimmIntroStep2 grimmIntroStep2;
    GrimmFinalIntroStep grimmFinalIntroStep;
    GrimmHalfHpStep grimmHalfHpStep;

    private float bossGroundY;

    public void Awake()
    {
        //player = PlayerManager.instance.player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;

        grimmIntroStep1 = new GrimmIntroStep1(this);
        grimmIntroStep2 = new GrimmIntroStep2(this);
        grimmFinalIntroStep = new GrimmFinalIntroStep(this);
        grimmHalfHpStep = new GrimmHalfHpStep(this);

        currentIntroStep = grimmIntroStep1;
    }

    public void Start()
    {
        TurnOffEffects();

        grimmIntroLightSP1.gameObject.SetActive(false);
        grimmSilhouette.gameObject.SetActive(false);
        fadeSprite.gameObject.SetActive(true);

        if (player.isInIntro)
        {
            fadeSprite.gameObject.SetActive(false);
        }
        BGMManager.instance.StopBGM();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeStep(CinematicStep.HalfHP);
        }
    }

    public void TurnOffEffects()
    {
        foreach (var eye in eyes)
        {
            eye.gameObject.SetActive(false);
        }

        particleEffect.Clear();
        grimmSilhouette.gameObject.SetActive(false);
        grimmIntroLightSP2.gameObject.SetActive(false);
        grimmIntroLight2.gameObject.SetActive(false);
        grimmTextCanvas.gameObject.SetActive(false);
        FVX.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == bossStartTrigger.transform && !player.isInIntro && isIntroPlay)
        {
            fadeSprite.StartSpriteFade(fadeSpriteDurtaion, 1f, 0f);

            isIntroPlay = false;
            bossGrimm.GetComponent<BossGrimm>().BossGrimmGreet();

            BGMManager.instance.SetBGM(bossBGM1, 0f, 1f);
            BGMManager.instance.ChangeBGM(0f, false);

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

        foreach (var backgroundScale in grimmPulsates)
        {
            backgroundScale.StartPulsate(pulseValue, pulseDelayTime);
        }

        Invoke("StartIntro", 2f);
    }

    public void StartIntro()
    {
        StartCoroutine(CameraMove());
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
                BGMManager.instance.BGMFadeOut();
                Invoke("PlayIntro", intro1StartDelay);
                break;
            }
        }
    }

    public void PlayIntro()
    {
        currentIntroStep.Enter();
    }

    public void ChangeStep(CinematicStep cinematicStepIndex, float delayTime = 0f)
    {
        switch (cinematicStepIndex)
        {
            case CinematicStep.Intro1:
                currentIntroStep = grimmIntroStep1;
                break;
            case CinematicStep.Intro2:
                currentIntroStep = grimmIntroStep2;
                break;
            case CinematicStep.FinalIntro:
                currentIntroStep = grimmFinalIntroStep;
                break;
            case CinematicStep.HalfHP:
                currentIntroStep = grimmHalfHpStep;
                break;
                //case CinematicStep.Outro1:
                //    currentIntroStep = grimmIntroStep1;
                //    break;
                //case CinematicStep.Outro2:
                //    currentIntroStep = grimmIntroStep1;
                //    break;
                //case CinematicStep.FinalOutro:
                //    currentIntroStep = grimmIntroStep1;
                //    break;
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
        BGMManager.instance.SetBGM(bossBGM2, 0f, 1f);
        BGMManager.instance.ChangeBGM(0f, false);

        Invoke("EffectFinish", finalIntroEffectDurtaion);
    }

    public void EffectFinish()
    {
        StartCoroutine(MoveToPlayer());
        grimmSilhouette.StartSpriteFade(canvasFadeOutDuration, 1f, 0f);
        TurnOffEffects();
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

        isBossStart = true;
        player.isInIntro = false;

        bossNightmareGrimm.SetActive(true);
        bossNightmareGrimm.GetComponent<BossGrimm>().BossGrimmNightmareStart(bossGroundY);
    }

    public void SwitchConfiner() => cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = bossCameraBoundingShape;
}
