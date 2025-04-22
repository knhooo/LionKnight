using UnityEngine;

public class BossGrimm : BossBase
{

    public int nextAttackType;
    public float attackEndTeleportDelay;

    public float teleportOutDelay;

    private bool firstBulletHell;

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
        // Debug.Log(stateMachine.currentState.animboolName);
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public void GrimmInVanish()
    {
        sr.enabled = false;
        cd.enabled = false;
        rb.simulated = false;
    }

    public void GrimmOutVanish()
    {
        sr.enabled = true;
        cd.enabled = true;
        rb.simulated = true;
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


        if (firstBulletHell)
        {
            // 탄막 지옥
            firstBulletHell = false;
            nextAttackType = 0;
        }
        else
        {
            // 일반패턴 1~4
            nextAttackType = Random.Range(1, 5);
        }
    }

    public void SetTeleportDelay(float delay)
    {
        teleportOutDelay = delay;
    }

    public void BossDeathTrigger()
    {
        Destroy(gameObject);
    }
}
