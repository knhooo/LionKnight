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

        boss.BossFloatPowerChange();
    }

    public override void Update()
    {
        base.Update();

        Vector2 movePos = new Vector2(boss.xPower, boss.yPower);
        boss.rb.linearVelocity = movePos;

        attackWait -= Time.deltaTime;
        upDownChangDelay -= Time.deltaTime;

        if ((boss.IsGroundDetected() || boss.IsCeilDetected()) && upDownChangDelay <= 0)
        {
            upDownChangDelay = boss.detectDelay;
            boss.isUp = !boss.isUp;
            boss.BossFloatPowerChange();
        }

        if (boss.IsWallDetected() && upDownChangDelay <= 0)
        {
            boss.ForcedFlip();
            boss.BossFloatPowerChange();
        }

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