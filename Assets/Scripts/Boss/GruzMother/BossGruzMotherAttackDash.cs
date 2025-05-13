using UnityEditor;
using UnityEngine;

public class BossGruzMotherAttackDash : BossGruzMotherState
{
    private float dashDuration;
    private int stateType;
    private Vector2 dashDir;
    private float reboundInvulTime; // 반동 후 충돌 무시 시간
    public BossGruzMotherAttackDash(BossGruzMother _boss, BossGruzMotherStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        dashDuration = boss.dashDuration;
        stateType = 1;
        reboundInvulTime = 0.2f;

        dashDir = (boss.playerTransform.position - boss.transform.position).normalized;
    }

    public override void Update()
    {
        base.Update();

        if(stateType == 1)
        {
            dashDuration -= Time.deltaTime;
            reboundInvulTime -= Time.deltaTime;

            // 돌진
            rb.linearVelocity = dashDir * boss.dashSpeed;

            // 벽, 천장, 바닥 감지
            if (boss.IsWallDetected() && reboundInvulTime <= 0)
            {
                rb.linearVelocity = new Vector2(-rb.linearVelocity.x, rb.linearVelocity.y).normalized * boss.dashReboundSpeed;
                stateType = 2;
                boss.LandEffGenerate(2);
            }
            if (boss.IsGroundDetected() && reboundInvulTime <= 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Abs(rb.linearVelocity.y)).normalized * boss.dashReboundSpeed;
                stateType = 2;
                boss.LandEffGenerate(3);
            }
            if (boss.IsCeilDetected() && reboundInvulTime <= 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -Mathf.Abs(rb.linearVelocity.y)).normalized * boss.dashReboundSpeed;
                stateType = 2;
                boss.LandEffGenerate(1);
            }

            // 벽, 천장, 바닥과 충돌
            if(stateType == 2)
            {
                boss.anim.SetTrigger("IsDashEnd");
                dashDuration = 0.3f;
                boss.BossWallCrashSound();
            }

            // 충돌 X
            if (dashDuration <= 0)
            {
                stateType = 3;
                boss.anim.SetTrigger("IsDashEnd");
            }
        }

        if(stateType == 2)
        {
            dashDuration -= Time.deltaTime;
            if (dashDuration <= 0)
            {
                stateType = 3;
            }
        }

        if (stateType == 3 && triggerCalled)
        {
            boss.stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.SetZeroVelocity();
    }
}
