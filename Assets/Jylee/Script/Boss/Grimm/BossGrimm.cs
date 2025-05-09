using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BossGrimm : BossBase
{

    [Header("수치 관련")]
    public bool isNightmare;
    public int nextAttackType;
    public float attackEndTeleportDelay;
    public float teleportOutDelay;
    public float groundY;
    public float bossDeadDelay;
    [SerializeField] private GameObject nGrimmIntro;
    [SerializeField] private int attackSelectMax;
    [SerializeField] private int attackSelectMin;

    [Header("공중 대쉬 공격")]
    public float adAirDashPower;
    public float adGroundDashPower;
    public float adLandDelay;
    public GameObject adDashTrail;

    [Header("대쉬 어퍼컷 공격")]
    public float duDashPower;
    public float duDashDistance;
    public float duUppercutPower;
    public float duUppercutDistance;
    [SerializeField] private GameObject duFireSpark;
    [SerializeField] private float fireSparkSpace;

    [Header("박쥐 소환 공격")]
    public Transform batFirePoint;
    public GameObject fireBatPrefab;
    public float firstShotDelay;
    public float secondShotDelay;
    public float thirdShotDelay;
    public float fourthShotDelay;
    public float shotEndDelay;

    [Header("가시 망토 공격")]
    public GameObject[] spikeList;
    public float spikeActionDuration;
    public float spikeUpDelay;
    public float spikeUpDuration;
    public float spikeSoundTiming;

    [Header("불 기둥 공격")]
    public GameObject firePillarCirclePrefab;
    public GameObject firePillarPrefab;
    public int firePillarCount;
    public float firePillarDelay;
    public float firePillarEndDelay;

    [Header("불릿 헬 공격")]
    public GameObject fireballPrefab;
    public int bulletFireCount;
    public float bulletFireDelay;
    private List<float> yPoints;
    private List<float[]> yTemplates;

    [Header("박쥐 변신 관련")]
    public GameObject grimmBatPrefab;
    public GameObject grimmBatShadowPrefab;
    public float grimmChangeBatDuration;
    public float grimmCombineTime;
    public int grimmBatShadowCount;
    private GameObject grimmBatObj;
    private List<GameObject> batShadowList;

    [Header("회피 관련")]
    public float evadeDistance;
    public float evadeSpeed;
    public float emeTeleportDistance;

    [Header("히트 박스")]
    public GameObject frontAttackPoint;
    public GameObject uppercutAttackPoint;
    public GameObject airDashAttackPoint;
    public GameObject frontDashAttackPoint;
    public GameObject bossBodyPoint;
    public GameObject bossSmallBodyPoint;

    [Header("위치 조정 관련")]
    [SerializeField] private float airDashYPos;
    [SerializeField] private float teleportPlayerShortDistanceMin;
    [SerializeField] private float teleportPlayerShortDistanceMax;
    [SerializeField] private float teleportPlayerMidDistanceMin;
    [SerializeField] private float teleportPlayerMidDistanceMax;
    [SerializeField] private Transform teleportLeftMax;
    [SerializeField] private Transform teleportRightMax;
    [SerializeField] private Transform bulletHellPos;
    [SerializeField] private Transform heightMax;
    [SerializeField] private Transform heightMin;
    [SerializeField] private Transform trailPos;

    [Header("이펙트")]
    [SerializeField] private GameObject deadEventPrefab;
    [SerializeField] private Transform airDashTransform;
    [SerializeField] private Transform groundDashTransform;
    [SerializeField] private Transform teleportEffTransform;
    [SerializeField] private GameObject airDashEff;
    [SerializeField] private GameObject groundDashEff;
    [SerializeField] private GameObject landEff;
    [SerializeField] private GameObject teleportEff;
    [SerializeField] private GameObject teleportSmokeEff;
    [SerializeField] private GameObject castAttackEff;
    [SerializeField] private GameObject castAttackParticle;

    private GameObject deadEventObj;
    private bool bulletHellTrigger;
    private bool firstBulletHell;
    private bool secondBulletHell;
    private bool firstBatChange;
    private bool useLandEff;
    private Coroutine loopRoutine;
    private Coroutine dashTrailCoroutine;

    public float bossGravity;

    public Transform playerTransform;

    private BossGrimmSoundClip soundClip;

    // 적의 상태를 관리하는 상태 머신
    public BossGrimmStateMachine stateMachine { get; private set; }

    // 적의 상태 (대기 상태, 이동 상태)
    public BossGrimmIdle idleState { get; private set; }
    public BossGrimmGreet greetState { get; private set; }
    public BossGrimmTeleportIn teleportInState { get; private set; }
    public BossGrimmTeleportOut teleportOutState { get; private set; }
    public BossGrimmAttackSelect attackSelectState { get; private set; }
    public BossGrimmAttackDashUppercut dashUppercutState { get; private set; }
    public BossGrimmAttackCast castState { get; private set; }
    public BossGrimmAttackAirDashAttack airDash { get; private set; }
    public BossGrimmAttackCapeSpike capeSpike { get; private set; }
    public BossGrimmAttackFirePillar firePillar { get; private set; }
    public BossGrimmWait waitState { get; private set; }
    public BossGrimmAttackBulletHell bulletHell { get; private set; }
    public BossGrimmChangeBat batState { get; private set; }
    public BossGrimmDeath deathState { get; private set; }

    private void Awake()
    {
        stateMachine = new BossGrimmStateMachine();

        idleState = new BossGrimmIdle(this, stateMachine, "IsIdle");
        greetState = new BossGrimmGreet(this, stateMachine, "IsGreet");
        teleportInState = new BossGrimmTeleportIn(this, stateMachine, "IsTeleportIn");
        teleportOutState = new BossGrimmTeleportOut(this, stateMachine, "IsTeleportOut");
        attackSelectState = new BossGrimmAttackSelect(this, stateMachine, "IsIdle");
        dashUppercutState = new BossGrimmAttackDashUppercut(this, stateMachine, "attackDashUppercut");
        castState = new BossGrimmAttackCast(this, stateMachine, "attackCast");
        airDash = new BossGrimmAttackAirDashAttack(this, stateMachine, "attackAirDash");
        capeSpike = new BossGrimmAttackCapeSpike(this, stateMachine, "attackCapeSpike");
        firePillar = new BossGrimmAttackFirePillar(this, stateMachine, "IsIdle");
        waitState = new BossGrimmWait(this, stateMachine, "IsIdle");
        bulletHell = new BossGrimmAttackBulletHell(this, stateMachine, "attackBulletHell");
        batState = new BossGrimmChangeBat(this, stateMachine, "IsBat");
        deathState = new BossGrimmDeath(this, stateMachine, "IsDeath");

    }

    protected override void Start()
    {
        base.Start();

        // 초기 상태를 대기 상태(idleState)로 설정
        stateMachine.Initalize(idleState);

        // 초기 값
        bulletHellTrigger = false;
        firstBulletHell = true;
        secondBulletHell = true;
        firstBatChange = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        soundClip = GetComponentInParent<BossGrimmSoundClip>();
        bossGravity = rb.gravityScale;
        yPoints = new List<float>();
        batShadowList = new List<GameObject>();

        if (isNightmare)
        {
            bossDeadDelay = 4.3f;
        }
        else
        {
            bossDeadDelay = soundClip.GrimmLongDefeatLength();
        }

        if (playerTransform == null)
        {
            playerTransform = gameObject.transform;
        }

        // 맵 y값 6등분
        for (int i = 0; i < 6; i++)
        {
            float y = Mathf.Lerp(heightMax.position.y, heightMin.position.y, (float)i / 5);
            yPoints.Add(y);
        }

        yTemplates = new List<float[]>
        {
            new float[] { yPoints[0], yPoints[2], yPoints[4] },
            new float[] { yPoints[0], yPoints[3], yPoints[4] },
            new float[] { yPoints[0], yPoints[4], yPoints[5] },
            new float[] { yPoints[1], yPoints[2], yPoints[4] },
            new float[] { yPoints[1], yPoints[3], yPoints[4] },
            new float[] { yPoints[1], yPoints[3], yPoints[5] },
            new float[] { yPoints[1], yPoints[4], yPoints[5] },
        };
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            bulletHellTrigger = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            stateMachine.ChangeState(batState);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            BossTakeDamage(1000);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            BossTakeDamage(10);
        }
    }

    protected override void BossCheckHealthPoint()
    {
        base.BossCheckHealthPoint();

        if (currentHealthPoint <= 0)
        {
            stateMachine.ChangeState(deathState);
            BGMManager.instance.BGMFadeOut();
            soundClip.GrimmFinalHit();
            if(isNightmare)
                soundClip.GrimmDefeatBgm();
        }
        else if (currentHealthPoint <= healthPoint / 2.5 && secondBulletHell)
        {
            bulletHellTrigger = true;
            secondBulletHell = false;
        }
        else if (currentHealthPoint <= healthPoint / 2 && firstBatChange)
        {
            stateMachine.ChangeState(batState);
            firstBatChange = false;

            if (isNightmare && nGrimmIntro != null)
            {
                nGrimmIntro.GetComponent<GrimmIntroController>().ChangeStep(CinematicStep.HalfHP);
            }
        }
        else if (currentHealthPoint <= healthPoint / 1.5 && firstBulletHell)
        {
            bulletHellTrigger = true;
            firstBulletHell = false;
        }
    }

    public void BossGrimmGreet()
    {
        stateMachine.ChangeState(greetState);
    }

    public void BossGrimmNightmareStart(float _groundY)
    {
        groundY = _groundY;
        SetTeleportDelay(attackEndTeleportDelay);

        foreach (GameObject i in spikeList)
        {
            i.GetComponentInChildren<Animator>().SetBool("IsNightmare", true);
        }

        Invoke("StartDelay", 0.5f);
    }

    private void StartDelay()
    {
        stateMachine.ChangeState(teleportInState);
        bossGravity = 1;
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    // 텔레포트 사라지기
    public void GrimmInVanish()
    {
        sr.enabled = false;
        cd.enabled = false;
        rb.simulated = false;
        TeleportEffGenerate();
        TeleportSmokeEffGenerate();
    }

    // 텔레포트 재등장
    public void GrimmOutVanish()
    {
        sr.enabled = true;
        cd.enabled = true;
        rb.simulated = true;
        TeleportEffGenerate();
        TeleportSmokeEffGenerate();
        soundClip.GrimmTeleportOut();
    }

    public void SelectGrimmAttack()
    {
        // 일반패턴
        // 1 : 대쉬 어퍼컷 공격
        // 2 : 박쥐 날리기
        // 3 : 공중 대쉬 -> 착지 대쉬
        // 4 : 망토 가시 공격

        // 탄막 지옥 패턴
        // 해당 패턴은 일정 비율의 체력이 빠지면 나옴
        // 지금 구상으로는 해당 함수에 진입시 체력 검사 했는데 50% 이하이고
        // bool값 firstBulletHell이 true 인 경우 false로 전환하고 탄막 지옥 패턴 실행

        useLandEff = false;

        if (bulletHellTrigger)
        {
            // 탄막 지옥
            bulletHellTrigger = false;
            nextAttackType = 0;
        }
        else
        {
            // 일반패턴 1~4
            nextAttackType = Random.Range(attackSelectMin, attackSelectMax);
            // nextAttackType = 4;
        }
    }

    // 텔레포트 딜레이 조정
    public void SetTeleportDelay(float delay)
    {
        teleportOutDelay = delay;
    }

    public void BossDeathTrigger()
    {
        deadEventObj.GetComponent<BossGrimmDeadEvent>().CancelCenterCircleGenerate();
        soundClip.GrimmExplodeIntoBats();
        if (nGrimmIntro != null && !isNightmare)
        {
            Invoke("NGrimmTrigger", 1f);
            sr.enabled = false;
            cd.enabled = false;
            rb.simulated = false;
        }
        else if(nGrimmIntro != null && isNightmare)
        {
            Invoke("NGrimmDeathTrigger", 1f);
            sr.enabled = false;
            cd.enabled = false;
            rb.simulated = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void NGrimmTrigger()
    {
        nGrimmIntro.GetComponent<GrimmIntroController>().NightmareGrimmTrigger();

        Destroy(gameObject);
    }

    private void NGrimmDeathTrigger()
    {
        nGrimmIntro.GetComponent<GrimmIntroController>().StartOutro();
    }

    // 이펙트 생성
    public void AirDashEffGenerate()
    {
        // 보스 위치 회전
        float bossRotation = anim.transform.eulerAngles.z;
        Quaternion effRotation = Quaternion.Euler(0f, 0f, bossRotation + 90f);
        
        Instantiate(airDashEff, airDashTransform.position, effRotation);
    }

    public void LandEffGenerate()
    {
        // 한번만
        if (useLandEff)
            return;

        useLandEff = true;

        // 좌
        Instantiate(landEff, groundDashTransform);

        // 우
        GameObject mirroEff = Instantiate(landEff, new Vector3(groundDashTransform.position.x, groundDashTransform.position.y), Quaternion.identity);
        Vector3 scale = mirroEff.transform.localScale;
        scale.x *= -1;
        mirroEff.transform.localScale = scale;
    }

    public void GroundDashEffGenerate()
    {
        if (facingLeft)
        {
            GameObject mirroEff = Instantiate(groundDashEff, new Vector3(groundDashTransform.position.x, groundDashTransform.position.y), Quaternion.identity);
            Vector3 scale = mirroEff.transform.localScale;
            scale.x *= -1;
            mirroEff.transform.localScale = scale;
        }
        else
        {
            Instantiate(groundDashEff, new Vector3(groundDashTransform.position.x, groundDashTransform.position.y), Quaternion.identity);
        }
    }

    public void BossFlip(bool reverse)
    {
        float gazePos = transform.position.x > playerTransform.position.x ? -1 : 1;
        gazePos = reverse ? gazePos * -1 : gazePos;

        // 좌우 반전 했을경우
        if (FlipController(gazePos, false))
        {
            // 박쥐 소환 포인트 뒤집기
            Vector3 firePointLocalPos = batFirePoint.localPosition;
            firePointLocalPos.x *= -1;
            batFirePoint.localPosition = firePointLocalPos;

            // 히트박스 뒤집기
            firePointLocalPos = frontAttackPoint.transform.localPosition;
            firePointLocalPos.x *= -1;
            frontAttackPoint.transform.localPosition = firePointLocalPos;
            frontAttackPoint.transform.localScale = new Vector3(frontAttackPoint.transform.localScale.x * -1, 1, 1);

            firePointLocalPos = uppercutAttackPoint.transform.localPosition;
            firePointLocalPos.x *= -1;
            uppercutAttackPoint.transform.localPosition = firePointLocalPos;
            uppercutAttackPoint.transform.localScale = new Vector3(uppercutAttackPoint.transform.localScale.x * -1, 1, 1);

            firePointLocalPos = frontDashAttackPoint.transform.localPosition;
            firePointLocalPos.x *= -1;
            frontDashAttackPoint.transform.localPosition = firePointLocalPos;
            frontDashAttackPoint.transform.localScale = new Vector3(frontDashAttackPoint.transform.localScale.x * -1, 1, 1);
        }
    }

    public void BossRandomTeleportSelect()
    {
        int leftRightSelect = Random.Range(1, 3);
        float xPosAdd = 0;
        float yPosAdd = 0;

        if(nextAttackType == 0)
        {
            rb.gravityScale = 0;
            transform.position = bulletHellPos.position;
            return;
        }
        else if(nextAttackType == 1 || nextAttackType == 3)
        {
            xPosAdd = Random.Range(teleportPlayerShortDistanceMin, teleportPlayerShortDistanceMax);
            if (nextAttackType == 3)
            {
                yPosAdd = airDashYPos;
                SetZeroVelocity();
                rb.gravityScale = 0;
            }
        }
        else if(nextAttackType == 2 || nextAttackType == 4 || nextAttackType == 5)
        {
            xPosAdd = Random.Range(teleportPlayerMidDistanceMin, teleportPlayerMidDistanceMax);
            if (nextAttackType == 5)
            {
                yPosAdd = airDashYPos;
                SetZeroVelocity();
                rb.gravityScale = 0;
            }
        }

        if (leftRightSelect == 1 && playerTransform.position.x + xPosAdd > teleportRightMax.position.x)
        {
            xPosAdd = xPosAdd * -1;
        }
        else if (leftRightSelect == 2 && playerTransform.position.x - xPosAdd > teleportLeftMax.position.x)
        {
            xPosAdd = xPosAdd * -1;
        }

        transform.position = new Vector3(playerTransform.position.x + xPosAdd, groundY + yPosAdd, 0);
    }

    public float BossPlayerGaze()
    {
        BossFlip(false);

        Vector2 direction = (new Vector3(playerTransform.position.x, groundY) - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        return angle;
    }

    public void BossRotationZero()
    {
        anim.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void BossCancelEverything()
    {
        anim.SetBool("attackDashUppercut", false);
        anim.SetBool("attackCast", false);
        anim.SetBool("attackAirDash", false);
        anim.SetBool("attackCapeSpike", false);

        frontAttackPoint.GetComponent<Collider2D>().enabled = false;
        uppercutAttackPoint.GetComponent<Collider2D>().enabled = false;
        airDashAttackPoint.GetComponent<Collider2D>().enabled = false;
        frontDashAttackPoint.GetComponent<Collider2D>().enabled = false;
        // 가시 캔슬
        foreach (GameObject i in spikeList)
        {
            i.GetComponentInChildren<Animator>().ResetTrigger("IsUp");
            i.GetComponentInChildren<Animator>().ResetTrigger("IsEnable");
            i.GetComponentInChildren<Animator>().ResetTrigger("IsDown");
            i.GetComponentInChildren<Animator>().Play("Empty");

            i.GetComponentInChildren<BossGrimmSpikeTrigger>().SpikeAttackDisable();
        }

        if (isNightmare)
        {
            BossGrimmDashTrailCoroutineEnd();
        }

        SetZeroVelocity();
        BossRotationZero();
    }

    public void BossFireBatFire()
    {
        GameObject fireBat = Instantiate(fireBatPrefab, batFirePoint.position, Quaternion.identity);
        Vector3 scale = fireBat.transform.localScale;
        scale.x = facingLeft ? 1 : -1;
        fireBat.transform.localScale = scale;

        // Instantiate(castAttackEff, batFirePoint.position, Quaternion.identity);
        CastParticleGenerate();
    }

    public void BossFireBatFireUp()
    {
        GameObject fireBat = Instantiate(fireBatPrefab, batFirePoint.position, Quaternion.identity);
        Vector3 scale = fireBat.transform.localScale;
        scale.x = facingLeft ? 1 : -1;
        fireBat.transform.localScale = scale;

        fireBat.GetComponent<BossGrimmFireBat>().UpFireBat();

        // Instantiate(castAttackEff, batFirePoint.position, Quaternion.identity);
        CastParticleGenerate();
    }

    public void BossCapeSpikeEnable()
    {
        foreach(GameObject i in spikeList)
        {
            // 가시 좌우 반전 정하기
            int temp = Random.Range(1, 3);
            float angleZ = temp == 1 ? 5f : -5f;
            i.transform.rotation = Quaternion.Euler(0, 0, angleZ);

            i.GetComponentInChildren<Animator>().SetTrigger("IsEnable");
        }

        soundClip.GrimmSpikesGrounded();
    }

    public void BossCapeUpSound()
    {
        soundClip.GrimmSpikesShootUp();
    }

    public void BossCapeDownSound()
    {
        soundClip.GrimmSpikesShrivelBack();
    }

    public void BossCapeSpikeUp()
    {
        foreach (GameObject i in spikeList)
        {
            i.GetComponentInChildren<Animator>().SetTrigger("IsUp");
        }
    }

    public void BossCapeSpikeDown()
    {
        foreach (GameObject i in spikeList)
        {
            i.GetComponentInChildren<Animator>().SetTrigger("IsDown");
        }
    }

    public void BossBulletHellSoundStartLoop()
    {
        loopRoutine = StartCoroutine(LoopBulletHellSoundRoutine());
    }

    public void BossBulletHellSoundStopLoop()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }

        soundClip.GrimmBalloonDeflate();
    }

    public void BossGrimmGreetSound()
    {
        soundClip.GrimmGreeting();
    }

    public void BossGrimmAppearSound()
    {
        soundClip.GrimmAppear();
    }

    public void BossGrimmEvadeSound()
    {
        soundClip.GrimmEvade();
    }

    public void BossGrimmFirePillarVoice()
    {
        soundClip.GrimmCast2();
    }

    public void BossGrimmDefeat()
    {
        deadEventObj = Instantiate(deadEventPrefab, teleportEffTransform.position, Quaternion.identity);
        if (isNightmare)
        {
            soundClip.GrimmScream();
        }
        else
        {
            soundClip.GrimmLongDefeat();
        }
    }

    public void BossGrimmScream()
    {
        soundClip.GrimmScream();
    }

    private IEnumerator LoopBulletHellSoundRoutine()
    {
        while (true)
        {
            soundClip.GrimmBalloonShootingFireLoop();
            yield return new WaitForSeconds(soundClip.GrimmBalloonShootingFireLoopLength());
        }
    }    

    public void TeleportEffGenerate()
    {
        Instantiate(teleportEff, teleportEffTransform.position, Quaternion.identity);
    }

    public void TeleportSmokeEffGenerate()
    {
        Vector3 temp = new Vector3(groundDashTransform.position.x, groundDashTransform.position.y + 0.5f);
        GameObject effect = Instantiate(teleportSmokeEff, temp, Quaternion.identity);
        Destroy(effect, 1f);
    }

    public void CastParticleGenerate()
    {
        Vector3 temp = new Vector3(batFirePoint.position.x, batFirePoint.position.y);
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f); // 위쪽으로 향하게
        GameObject effect = Instantiate(castAttackParticle, temp, rotation);
        Destroy(effect, 1f);
    }

    public void BossGrimmSparkGenerate()
    {
        Vector3 temp = teleportEffTransform.position;
        GameObject center = Instantiate(duFireSpark, teleportEffTransform.position, Quaternion.identity);

        GameObject leftFirst = Instantiate(duFireSpark, teleportEffTransform.position, Quaternion.identity);

        GameObject leftSecond = Instantiate(duFireSpark, teleportEffTransform.position, Quaternion.identity);

        GameObject rightFirst = Instantiate(duFireSpark, teleportEffTransform.position, Quaternion.identity);

        GameObject rightSecond = Instantiate(duFireSpark, teleportEffTransform.position, Quaternion.identity);

        Rigidbody2D rbCenter = center.GetComponent<Rigidbody2D>();
        rbCenter.linearVelocity = new Vector2(0f, 0);

        Rigidbody2D rbLeftFirst = leftFirst.GetComponent<Rigidbody2D>();
        rbLeftFirst.linearVelocity = new Vector2(-fireSparkSpace, 0);

        Rigidbody2D rbLeftSecond = leftSecond.GetComponent<Rigidbody2D>();
        rbLeftSecond.linearVelocity = new Vector2(-fireSparkSpace * 2, 0);

        Rigidbody2D rbRightFirst = rightFirst.GetComponent<Rigidbody2D>();
        rbRightFirst.linearVelocity = new Vector2(fireSparkSpace, 0);

        Rigidbody2D rbRightSecond = rightSecond.GetComponent<Rigidbody2D>();
        rbRightSecond.linearVelocity = new Vector2(fireSparkSpace * 2, 0);
    }

    public void BossGrimmBulletHellGenerate()
    {
        float[] selectedPointsRight = yTemplates[Random.Range(0, yTemplates.Count)];
        float[] selectedPointsLeft = yTemplates[Random.Range(0, yTemplates.Count)];

        foreach (float yTarget in selectedPointsRight)
        {
            FireBulletLeftRight(yTarget, true);
        }

        foreach (float yTarget in selectedPointsLeft)
        {
            FireBulletLeftRight(yTarget, false);
        }

        FireBulletDown();
    }

    private void FireBulletLeftRight(float targetY, bool isRight)
    {
        float distance = isRight ? 10f : -10f;

        //Vector2 startPos = bulletHellPos.position;
        Vector2 startPos = new Vector2(teleportEffTransform.position.x, teleportEffTransform.position.y); ;

        Vector2 targetPos = new Vector2(startPos.x + distance, targetY); // 오른쪽 방향 + y 목표

        Vector2 direction = (targetPos - startPos).normalized;

        GameObject bullet = Instantiate(fireballPrefab, startPos, Quaternion.identity);

        Vector3 scale = bullet.transform.localScale;
        scale.x = isRight ? 1 : -1;
        bullet.transform.localScale = scale;

        bullet.GetComponent<BossGrimmFireball>().Init(targetY, isRight);
    }

    private void FireBulletDown()
    {
        float randomSpeed = Random.Range(2f, 4f);
        Vector2 moveDir = Vector2.down;
        // 중앙
        Vector2 startPos = bulletHellPos.position;
        Vector2 targetPos = new Vector2(startPos.x, startPos.y);
        GameObject bullet1 = Instantiate(fireballPrefab, targetPos, Quaternion.identity);
        bullet1.GetComponent<Rigidbody2D>().linearVelocity = moveDir * randomSpeed;

        randomSpeed = Random.Range(2f, 4f);
        // 오른쪽
        targetPos = new Vector2(startPos.x + 0.5f, startPos.y);
        GameObject bullet2 = Instantiate(fireballPrefab, targetPos, Quaternion.identity);
        bullet2.GetComponent<Rigidbody2D>().linearVelocity = moveDir * randomSpeed;

        randomSpeed = Random.Range(2f, 4f);
        // 왼쪽
        targetPos = new Vector2(startPos.x - 0.5f, startPos.y);
        GameObject bullet3 = Instantiate(fireballPrefab, targetPos, Quaternion.identity);
        bullet3.GetComponent<Rigidbody2D>().linearVelocity = moveDir * randomSpeed;
    }

    public void BossGrimmSplitBat()
    {
        soundClip.GrimmExplodeIntoBats();
        soundClip.GrimmBatsCircling();

        grimmBatObj = Instantiate(grimmBatPrefab, teleportEffTransform.position, Quaternion.identity);
        grimmBatObj.GetComponent<BossGrimmBat>().GetMainBossObj(gameObject);
        for (int i = 0; i < grimmBatShadowCount; i++)
        {
            GameObject obj = Instantiate(grimmBatShadowPrefab, teleportEffTransform.position, Quaternion.identity);
            batShadowList.Add(obj);
        }
    }

    public void BossGrimmCombineBat()
    {
        soundClip.GrimmBatsReform();
        grimmBatObj.GetComponent<BossGrimmBat>().CombineTrigger(teleportEffTransform);
        foreach(GameObject obj in batShadowList)
        {
            obj.GetComponent<BossGrimmBat>().CombineTrigger(teleportEffTransform);
        }

        batShadowList.Clear();
    }

    public void BossGrimmDashTrailCoroutineStart()
    {
        if (dashTrailCoroutine == null)
        {
            dashTrailCoroutine = StartCoroutine(BossGrimmTrailGenerate());
        }
    }

    public void BossGrimmDashTrailCoroutineEnd()
    {
        if (dashTrailCoroutine != null)
        {
            StopCoroutine(dashTrailCoroutine);
            dashTrailCoroutine = null;
        }
    }

    private IEnumerator BossGrimmTrailGenerate()
    {
        while (true)
        {
            Instantiate(adDashTrail, trailPos.position, Quaternion.identity);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void BossGrimmFirePillarGenerate()
    {
        GameObject fireCircle = Instantiate(firePillarCirclePrefab, new Vector3(playerTransform.position.x, groundY + 0.2f), Quaternion.identity);
    }

    public void BossGroggy()
    {
        //이펙트
        GameObject obj = Instantiate(hitCrack, teleportEffTransform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        //카메라 쉐이크
        if (cineCam.GetComponent<CameraShake>() != null)
            cineCam.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude, shakeFrequency, shakeDuration);
        //시간느려지는효과
        StartCoroutine(HitStop(0.3f, 0.2f));
    }

    IEnumerator HitStop(float duration, float slowTimeScale)
    {
        Time.timeScale = slowTimeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}
