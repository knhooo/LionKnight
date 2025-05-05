using System.Collections;
using System.Diagnostics;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public enum CinematicStep
{
    None, Intro1, Intro2, Intro3, FinalIntro_UI, HalfHP, Outro1, Outro2, Outro3, Outro4, FinalOutro
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
    [SerializeField] public GrimmFadeSprite fadeSprite;
    [SerializeField] public float fadeSpriteDurtaion;

    [Header("카메라")]
    [SerializeField] public GameObject cinemachineCamera;
    [SerializeField] public BoxCollider2D bossCameraBoundingShape;
    [SerializeField] public Transform[] CameraPos;
    [SerializeField] public float cameraMoveTime = 0.5f;

    [Space]
    [Header("BGM")]
    [SerializeField] public float BGM1StartDelay = 0.7f;
    [SerializeField] public AudioClip[] bossBGM1;
    [SerializeField] public AudioClip[] bossBGM2;
    [SerializeField] public AudioClip bossBusrtAudio;
    [SerializeField] public AudioClip burstAudio1;
    [SerializeField] public AudioClip burstAudio2;
    [SerializeField] public AudioClip burstAudio3;
    [SerializeField] public FadeObject burstAudioLoop;

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

    [Header("그림 실루엣")]
    [SerializeField] public GrimmSilhouette grimmSilhouette;
    [SerializeField] public float grimmSilhouetteActiveTime = 1f;
    [SerializeField] public float grimmSilhouetteGrowSpeed = 0.5f;
    [SerializeField] public float grimmSilhouetteOverGrowSpeed = 0.5f;

    [Space]
    [Header("마지막효과 - UI")]
    [SerializeField] public Canvas grimmTextCanvas;
    [SerializeField] public float finalIntroEffectDurtaion = 1.7f;
    [SerializeField] public float moveToPlayerTime = 0.5f;
    [SerializeField] public float canvasFadeOutDuration = 0.5f;

    [Space]
    [Header("보스 체력 절반 이하")]
    [SerializeField] public FadeObject[] eyes;
    [SerializeField] public float introEyesLightIntensity = 0.5f;
    [SerializeField] public float fadeEyesDuration;
    [SerializeField] public float newPulseValue;
    [SerializeField] public float newPulseDalyTime;

    [Space]
    [Header("아웃트로1")]
    [SerializeField] public float outro1StartDelay = 1f;
    [SerializeField] public float outro1FadeSpriteDuration = 0.5f;
    [SerializeField] public float outro1FadeSpriteAlpha = 0.5f;

    [Header("카메라 Shake")]
    [SerializeField] public float outroShake1Amplitude = 4f;
    [SerializeField] public float outroShake1Frequency3 = 30f;
    [SerializeField] public float spriteFadeDuration;

    [Space]
    [Header("아웃트로2")]
    [SerializeField] public float outro2StartDelay = 1f;
    [SerializeField] public float outroEyesLightIntensity = 2f;

    [Space]
    [Header("아웃트로3")]
    [SerializeField] public float outro3StartDelay = 1f;
    [SerializeField] public float outroSilhouetteFadeInDuration = 2f;
    [SerializeField] public GameObject outroFVX;

    [Header("카메라 Shake")]
    [SerializeField] public float outro3Shake3Amplitude = 4f;
    [SerializeField] public float outro3ShakeFrequency = 30f;

    [Space]
    [Header("아웃트로4")]
    [SerializeField] public float outro4StartDelay = 1f;
    [SerializeField] public FadeObject outroFadeSprite;
    [SerializeField] public float outroFadeSpriteDuration = 1f;


    [Space]
    [Header("아웃트로5")]
    [SerializeField] public float finalOutroStartDelay = 1f;
    [SerializeField] public float burstAudioLoopFadeOutDuration = 0.5f;
    [SerializeField] public float outroSilhouetteFadeOutDuration = 0.5f;


    [HideInInspector] public Player player;
    [HideInInspector] public Vector3 destination;

    [HideInInspector] public bool isBossStart = false;
    [HideInInspector] public bool isOutro = false;

    private GrimmCinematicStep currentIntroStep;
    private GrimmIntroStep1 grimmIntroStep1;
    private GrimmIntroStep2 grimmIntroStep2;
    private GrimmIntroStep3 grimmFinalIntroStep;
    private FinalGrimmIntro_UI finalGrimmIntro_UI;
    private GrimmHalfHpStep grimmHalfHpStep;
    private GrimmOutroStep1 grimmOutroStep1;
    private GrimmOutroStep2 grimmOutroStep2;
    private GrimmOutroStep3 grimmOutroStep3;
    private GrimmOutroStep4 grimmOutroStep4;
    private FinalGrimmOutroStep finalGrimmOutro;

    private float bossGroundY;

    public void Awake()
    {
        //player = PlayerManager.instance.player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        transform.SetParent(player.transform);
        transform.localPosition = Vector3.zero;

        grimmIntroStep1 = new GrimmIntroStep1(this);
        grimmIntroStep2 = new GrimmIntroStep2(this);
        grimmFinalIntroStep = new GrimmIntroStep3(this);
        finalGrimmIntro_UI = new FinalGrimmIntro_UI(this);
        grimmHalfHpStep = new GrimmHalfHpStep(this);
        grimmOutroStep1 = new GrimmOutroStep1(this);
        grimmOutroStep2 = new GrimmOutroStep2(this);
        grimmOutroStep3 = new GrimmOutroStep3(this);
        grimmOutroStep4 = new GrimmOutroStep4(this);
        finalGrimmOutro = new FinalGrimmOutroStep(this);

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartOutro();
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
        outroFVX.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == bossStartTrigger.transform && !player.isInIntro && isIntroPlay)
        {
            SwitchConfiner();
            fadeSprite.StartSpriteFade(fadeSpriteDurtaion, 1f, 0f);

            isIntroPlay = false;
            bossGrimm.GetComponent<BossGrimm>().BossGrimmGreet();

            BGMManager.instance.SetBGM(bossBGM1, 0f, 1f);
            BGMManager.instance.ChangeBGM(BGM1StartDelay, false);

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
        BGMManager.instance.BGMFadeOut();

        bossStartTrigger.gameObject.SetActive(false);

        foreach (var backgroundScale in grimmPulsates)
        {
            backgroundScale.StartPulsate(pulseValue, pulseDelayTime);
        }

        Invoke("StartCameraMove", 2f);
    }

    public void StartCameraMove()
    {
        StartCoroutine(CameraMove());
    }

    private IEnumerator CameraMove()
    {
        while (true)
        {
            for (int i = 0; i < CameraPos.Length; i++)
            {
                if (isOutro)
                    i = CameraPos.Length - 1;

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
                if (!isOutro)
                {
                    Invoke("PlayIntro", intro1StartDelay);
                    break;
                }
            }
        }
    }

    private void PlayIntro()
    {
        currentIntroStep.Enter();
    }

    public void StartOutro()
    {
        ChangeStep(CinematicStep.Outro1, outro1StartDelay);
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
            case CinematicStep.Intro3:
                currentIntroStep = grimmFinalIntroStep;
                break;
            case CinematicStep.FinalIntro_UI:
                currentIntroStep = finalGrimmIntro_UI;
                break;
            case CinematicStep.HalfHP:
                currentIntroStep = grimmHalfHpStep;
                break;
            case CinematicStep.Outro1:
                currentIntroStep = grimmOutroStep1;
                break;
            case CinematicStep.Outro2:
                currentIntroStep = grimmOutroStep2;
                break;
            case CinematicStep.Outro3:
                currentIntroStep = grimmOutroStep3;
                break;
            case CinematicStep.Outro4:
                currentIntroStep = grimmOutroStep4;
                break;
            case CinematicStep.FinalOutro:
                currentIntroStep = finalGrimmOutro;
                break;
        }
        StartCoroutine(StartNextStep(delayTime));
    }

    private IEnumerator StartNextStep(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        currentIntroStep.Enter();
    }

    public void CallIntroFinish()
    {
        Invoke("IntroFinish", finalIntroEffectDurtaion);
    }

    private void IntroFinish()
    {
        StartCoroutine(MoveCameraToPlayer());
        grimmSilhouette.GetComponent<FadeObject>().StartSpriteFade(canvasFadeOutDuration, 1f, 0f);
        TurnOffEffects();
    }

    public IEnumerator MoveCameraToPlayer()
    {
        yield return new WaitForSeconds(canvasFadeOutDuration);

        float elapsed = 0f;
        while (elapsed <= moveToPlayerTime)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, destination, elapsed / moveToPlayerTime);
            yield return null;
        }

        isBossStart = true;
        player.isInIntro = false;

        bossNightmareGrimm.SetActive(true);
        bossNightmareGrimm.GetComponent<BossGrimm>().BossGrimmNightmareStart(bossGroundY);
    }

    public void SwitchConfiner() => cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = bossCameraBoundingShape;
}
