using UnityEngine;

public class BossGrimmTeleportIn : BossGrimmState
{

    public BossGrimmTeleportIn(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
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
            boss.GrimmInVanish();

            boss.stateMachine.ChangeState(boss.attackSelectState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
