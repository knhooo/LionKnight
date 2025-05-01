using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BossGrimmAttackAirDashAttack : BossGrimmState
{
    private bool isAirDash;
    private float landDelay;
    private float finishWait;
    private float playerAngle;
    private Vector3 playerLocation;

    // 1 : 공중대쉬 준비상태
    // 2 : 공중대쉬 공격중
    // 3 : 지상착지중
    // 4 : 지상착지 딜레이
    // 5 : 지상대쉬 공격중
    // 6 : 지상대쉬 공격후 착지
    private int stateType;

    public BossGrimmAttackAirDashAttack(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isAirDash = false;
        stateType = 1;
        landDelay = boss.adLandDelay;
        finishWait = 0.5f;

        playerAngle = boss.BossPlayerGaze();
        playerLocation = new Vector3(boss.playerTransform.position.x, boss.groundY);
    }

    public override void Update()
    {
        base.Update();

        // 공중 대쉬 준비 끝
        if (triggerCalled && stateType == 1)
        {
            triggerCalled = false;

            // 플레이어 위치로 회전
            // 시선이 아래쪽 이여서 +90도를 하여 플레이어를 바라보게함
            boss.anim.transform.rotation = Quaternion.Euler(0, 0, playerAngle + 90);

            // 공중 대쉬로 전환
            boss.anim.SetTrigger("attackAirDashing");
            isAirDash = true;
            stateType = 2;

            // 악몽의 왕인 경우 트레일 생성
            if(boss.isNightmare)
                boss.BossGrimmDashTrailCoroutineStart();

            // 수치만큼 돌진
            Vector2 airDashDirection = (playerLocation - boss.transform.position).normalized;
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

            // 악몽의 왕인 경우 트레일 중지
            if (boss.isNightmare)
                boss.BossGrimmDashTrailCoroutineEnd();

            // 착지 모션 전환
            stateType = 3;
            boss.anim.SetTrigger("attackAirDashLanding");
        }

        // 착지 후 딜레이
        if (isAirDash && triggerCalled && stateType == 3)
        {
            landDelay -= Time.deltaTime;
            if(landDelay <= 0)
            {
                stateType = 4;
            }
        }

        // 착지 후 대쉬 전환
        if (isAirDash && triggerCalled && stateType == 4)
        {
            triggerCalled = false;
            stateType = 5;
            boss.anim.SetTrigger("attackAirDashLandingToDash");
            rb.linearVelocity = new Vector2(boss.facingLeft ? boss.adGroundDashPower : -boss.adGroundDashPower, rb.linearVelocityY);

            // 악몽의 왕인 경우 트레일 생성
            if (boss.isNightmare)
                boss.BossGrimmDashTrailCoroutineStart();
        }

        // 착지 대쉬 후 랜딩 전환
        if(stateType == 5 && triggerCalled)
        {
            stateType = 6;
            boss.anim.SetTrigger("attackAirDashLanding");
            boss.SetZeroVelocity();

            // 악몽의 왕인 경우 트레일 중지
            if (boss.isNightmare)
                boss.BossGrimmDashTrailCoroutineEnd();
        }

        // 랜딩 대기시간
        if(stateType == 6)
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
