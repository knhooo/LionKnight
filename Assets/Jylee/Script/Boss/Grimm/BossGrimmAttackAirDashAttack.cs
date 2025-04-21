using UnityEditor;
using UnityEngine;

public class BossGrimmAttackAirDashAttack : BossGrimmState
{
    private bool isAirDash;
    private float airDashSpeed;
    private float landingDashSpeed;
    private float landingDashDuration;
    private float finishWait;
    private float tempTimer;

    // 1 : 공중대쉬 준비상태
    // 2 : 공중대쉬 공격중
    // 3 : 지상착지중
    // 4 : 지상대쉬 공격중
    // 5 : 지상대쉬 공격후 착지
    private int stateType;
    public BossGrimmAttackAirDashAttack(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isAirDash = false;
        airDashSpeed = 3f;
        landingDashSpeed = 3f;
        landingDashDuration = 1f;
        tempTimer = 0.3f;
        stateType = 1;
        finishWait = 0.5f;
    }

    public override void Update()
    {
        base.Update();

        // 공중 대쉬 준비 끝
        if (triggerCalled && stateType == 1)
        {
            // 공중 대쉬로 전환
            stateType = 2;
            triggerCalled = false;
            isAirDash = true;
            boss.anim.SetTrigger("attackAirDashing");
        }

        // 임시코드 나중에 착지 제어 추가하면 삭제
        if (isAirDash && stateType == 2)
        {
            tempTimer -= Time.deltaTime;
        }

        // && 착지 제어 추가
        if (isAirDash && stateType == 2 && tempTimer <= 0)
        {
            // 착지 모션 전환
            stateType = 3;
            boss.anim.SetTrigger("attackAirDashLanding");
        }

        // 착지 후 대쉬 전환
        if(isAirDash && triggerCalled && stateType == 3)
        {
            triggerCalled = false;
            stateType = 4;
            boss.anim.SetTrigger("attackAirDashLandingToDash");
        }

        // 착지 대쉬 후 랜딩 전환
        if(stateType == 4 && triggerCalled)
        {
            stateType = 5;
            boss.anim.SetTrigger("attackAirDashLanding");
        }

        // 랜딩 대기시간
        if(stateType == 5)
        {
            finishWait -= Time.deltaTime;
        }

        // 대기시간 종료 시 패턴 종료
        if(finishWait <= 0)
        {
            boss.stateMachine.ChangeState(boss.teleportInState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
