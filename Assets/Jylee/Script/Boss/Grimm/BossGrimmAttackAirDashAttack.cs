using UnityEditor;
using UnityEngine;

public class BossGrimmAttackAirDashAttack : BossGrimmState
{
    private bool isAirDash;
    private float finishWait;

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
        stateType = 1;
        finishWait = 0.5f;
    }

    public override void Update()
    {
        base.Update();

        // 공중 대쉬 준비 끝
        if (triggerCalled && stateType == 1)
        {
            triggerCalled = false;

            // 플레이어 위치로 회전
            boss.BossPlayerGaze();

            // 공중 대쉬로 전환
            boss.anim.SetTrigger("attackAirDashing");
            isAirDash = true;
            stateType = 2;

            // 수치만큼 돌진
            Vector2 airDashDirection = (new Vector3(boss.playerTransform.position.x, boss.groundY) - boss.transform.position).normalized;
            rb.linearVelocity = airDashDirection * boss.adAirDashPower;
        }

        // && 착지 제어 추가
        if (isAirDash && stateType == 2 && boss.IsGroundDetected())
        {
            // 회전값 정상화
            boss.BossRotationZero();
            // 이동값 정상화
            boss.SetZeroVelocity();
            // 중력 정상화
            boss.rb.gravityScale = boss.bossGravity;
            // 플레이어 위치에 따른 뒤집기
            boss.BossFlip(false);

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
            rb.linearVelocity = new Vector2(boss.facingLeft ? boss.adGroundDashPower : -boss.adGroundDashPower, rb.linearVelocityY);
        }

        // 착지 대쉬 후 랜딩 전환
        if(stateType == 4 && triggerCalled)
        {
            stateType = 5;
            boss.anim.SetTrigger("attackAirDashLanding");
            boss.SetZeroVelocity();
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
