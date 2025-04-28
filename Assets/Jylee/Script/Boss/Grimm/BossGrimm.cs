using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class BossGrimm : BossBase
{

    [Header("수치 관련")]
    public int nextAttackType;
    public float attackEndTeleportDelay;
    public float teleportOutDelay;
    public float groundY;

    [Header("공중 대쉬 공격")]
    public float adAirDashPower;
    public float adGroundDashPower;

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
    public float shotEndDelay;

    [Header("가시 망토 공격")]
    public GameObject[] spikeList;

    [Header("회피 관련")]
    public float evadeDistance;
    public float evadeSpeed;
    public float emeTeleportDistance;

    [Header("위치 조정 관련")]
    [SerializeField] private float airDashYPos;
    [SerializeField] private float teleportPlayerShortDistanceMin;
    [SerializeField] private float teleportPlayerShortDistanceMax;
    [SerializeField] private float teleportPlayerMidDistanceMin;
    [SerializeField] private float teleportPlayerMidDistanceMax;
    [SerializeField] private Transform teleportLeftMax;
    [SerializeField] private Transform teleportRightMax;
    [SerializeField] private Transform bulletHellPos;

    private bool firstBulletHell;
    private bool useLandEff;

    public float bossGravity;

    [Header("이펙트")]
    [SerializeField] private Transform airDashTransform;
    [SerializeField] private Transform groundDashTransform;
    [SerializeField] private Transform teleportEffTransform;
    [SerializeField] private GameObject airDashEff;
    [SerializeField] private GameObject groundDashEff;
    [SerializeField] private GameObject landEff;
    [SerializeField] private GameObject teleportEff;
    [SerializeField] private GameObject teleportSmokeEff;
    [SerializeField] private GameObject castAttackEff;

    public Transform playerTransform;

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
        firstBulletHell = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        bossGravity = rb.gravityScale;

        if (playerTransform == null)
        {
            playerTransform = gameObject.transform;
        }
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            firstBulletHell = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            stateMachine.ChangeState(batState);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            stateMachine.ChangeState(deathState);
        }
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

        if (firstBulletHell)
        {
            // 탄막 지옥
            firstBulletHell = false;
            nextAttackType = 0;
        }
        else
        {
            // 일반패턴 1~4
            // nextAttackType = Random.Range(1, 5);
            nextAttackType = 2;
        }
    }

    // 텔레포트 딜레이 조정
    public void SetTeleportDelay(float delay)
    {
        teleportOutDelay = delay;
    }

    public void BossDeathTrigger()
    {
        Destroy(gameObject);
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
        if (FlipController(gazePos))
        {
            // 박쥐 소환 포인트 뒤집기
            Vector3 firePointLocalPos = batFirePoint.localPosition;
            firePointLocalPos.x *= -1;
            batFirePoint.localPosition = firePointLocalPos;
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
                rb.gravityScale = 0;
            }
        }
        else if(nextAttackType == 2 || nextAttackType == 4)
        {
            xPosAdd = Random.Range(teleportPlayerMidDistanceMin, teleportPlayerMidDistanceMax);
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
        Vector2 direction = (new Vector3(playerTransform.position.x, groundY) - transform.position).normalized; // 플레이어 방향 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 각도로 변환

        return angle;

        // 시선이 위쪽이여서 -90도를 하여 플레이어를 바라보게함
        // anim.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }
    public void BossRotationZero()
    {
        anim.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void BossFireBatFire()
    {
        GameObject fireBat = Instantiate(fireBatPrefab, batFirePoint.position, Quaternion.identity);
        Vector3 scale = fireBat.transform.localScale;
        scale.x = facingLeft ? 1 : -1;
        fireBat.transform.localScale = scale;

        Instantiate(castAttackEff, batFirePoint.position, Quaternion.identity);
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
}
