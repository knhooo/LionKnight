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

        // 애니메이션 끝나면 바로 전환
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
