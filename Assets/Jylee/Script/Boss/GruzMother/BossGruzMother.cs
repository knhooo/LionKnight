using UnityEngine;
using UnityEngine.Rendering;

public class BossGruzMother : BossBase
{

    public bool isSleep;

    [Header("수치 관련")]
    public float bossSpeed;
    public float bossAttackDelay;
    public float xPower;
    public float yPower;
    public bool isUp;
    public float detectDelay;

    [Header("도움 닫기")]
    public float runUpSpeed;
    public float runUpDuration;

    [Header("대쉬 공격")]
    public float dashSpeed;
    public float dashDuration;
    public float dashReboundSpeed;

    [Header("슬램 공격")]
    public float slamXSpeed;
    public float slamYSpeed;
    public int slamCount;

    [Header("자폭")]
    public GameObject gruzPrefab;
    public int gruzSpawnCount;

    [Header("이펙트 관련")]
    public GameObject reboundEff;

    public Transform playerTransform;


    // 적의 상태를 관리하는 상태 머신
    public BossGruzMotherStateMachine stateMachine { get; private set; }

    // 적의 상태 (대기 상태, 이동 상태)
    public BossGruzMotherSleep sleepState { get; private set; }
    public BossGruzMotherAwake awakeState { get; private set; }
    public BossGruzMotherIdle idleState { get; private set; }
    public BossGruzMotherAttackCharge attackCharge { get; private set; }
    public BossGruzMotherAttackDash dashAttack { get; private set; }

    private void Awake()
    {
        stateMachine = new BossGruzMotherStateMachine();

        sleepState = new BossGruzMotherSleep(this, stateMachine, "IsSleep");
        awakeState = new BossGruzMotherAwake(this, stateMachine, "IsAwake");
        idleState = new BossGruzMotherIdle(this, stateMachine, "IsIdle");
        attackCharge = new BossGruzMotherAttackCharge(this, stateMachine, "IsAttackReady");
        dashAttack = new BossGruzMotherAttackDash(this, stateMachine, "IsDashAttack");
    }

    protected override void Start()
    {
        base.Start();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 초기 상태를 대기 상태(idleState)로 설정
        stateMachine.Initalize(sleepState);

        isSleep = true;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    protected override void BossCheckHealthPoint()
    {
        base.BossCheckHealthPoint();

        if (isSleep)
        {
            isSleep = false;
            stateMachine.ChangeState(awakeState);
        }
    }

    public void ForcedFlip()
    {
        FlipReverse();

        // 벽 감지 뒤집기
        Vector3 wallCheckLocalPos = wallCheck.localPosition;
        wallCheckLocalPos.x *= -1;
        wallCheck.localPosition = wallCheckLocalPos;
    }

    public void BossFlip(bool reverse)
    {
        float gazePos = transform.position.x > playerTransform.position.x ? -1 : 1;
        gazePos = reverse ? gazePos * -1 : gazePos;

        // 좌우 반전 했을경우
        if (FlipController(gazePos, reverse))
        {
            // 벽 감지 뒤집기
            Vector3 wallCheckLocalPos = wallCheck.localPosition;
            wallCheckLocalPos.x *= -1;
            wallCheck.localPosition = wallCheckLocalPos;
        }
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public void BossFloatPowerChange()
    {
        float temp = Random.Range(1f, bossSpeed);
        xPower = facingLeft ? -temp : temp;
        yPower = isUp ? bossSpeed - temp : (bossSpeed - temp) * -1;
    }
}
