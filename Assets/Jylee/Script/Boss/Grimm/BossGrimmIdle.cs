using UnityEngine;

public class BossGrimmIdle : BossGrimmState
{
    public BossGrimmIdle(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        boss.BossFlip(false);

        if (Input.GetKeyDown(KeyCode.A))
        {
            stateMachine.ChangeState(boss.greetState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
