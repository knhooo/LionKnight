using UnityEngine;

public class BossGruzMotherIdle : BossGruzMotherState
{
    private float attackWait;
    private float upDownChangDelay;
    public BossGruzMotherIdle(BossGruzMother _boss, BossGruzMotherStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        attackWait = boss.bossAttackDelay;
        upDownChangDelay = boss.detectDelay;
        // 이동 방향 선택
        boss.BossFloatPowerChange();
    }

    public override void Update()
    {
        base.Update();

        if (boss.isDead)
            return;

        // 이동
        Vector2 movePos = new Vector2(boss.xPower, boss.yPower);
        boss.rb.linearVelocity = movePos;

        attackWait -= Time.deltaTime;
        upDownChangDelay -= Time.deltaTime;

        // 천장 바닥 감지
        if ((boss.IsGroundDetected() || boss.IsCeilDetected()) && upDownChangDelay <= 0)
        {
            upDownChangDelay = boss.detectDelay;
            boss.isUp = !boss.isUp;
            boss.BossFloatPowerChange();
        }

        // 벽 감지
        if (boss.IsWallDetected() && upDownChangDelay <= 0)
        {
            boss.ForcedFlip();
            boss.BossFloatPowerChange();
        }

        // 지속시간 끝나면 공격 선택
        if (attackWait <= 0)
        {
            boss.stateMachine.ChangeState(boss.attackCharge);
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.SetZeroVelocity();
    }
}