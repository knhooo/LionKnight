using UnityEngine;

public class BossGruzMotherAwake : BossGruzMotherState
{
    public BossGruzMotherAwake(BossGruzMother _boss, BossGruzMotherStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            boss.stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
