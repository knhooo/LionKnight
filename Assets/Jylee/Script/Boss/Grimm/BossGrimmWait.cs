using UnityEngine;

public class BossGrimmWait : BossGrimmState
{
    private float delay;
    public BossGrimmWait(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        delay = boss.shotEndDelay;
    }

    public override void Update()
    {
        base.Update();

        delay -= Time.deltaTime;
        if(delay <= 0)
        {
            boss.stateMachine.ChangeState(boss.teleportInState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
