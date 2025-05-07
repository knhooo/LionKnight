using UnityEngine;

public class BossGruzMotherAttackCharge : BossGruzMotherState
{
    public BossGruzMotherAttackCharge(BossGruzMother _boss, BossGruzMotherStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        boss.BossFlip(true);
    }

    public override void Update()
    {
        base.Update();

        Vector2 retreatDir = (boss.transform.position - boss.playerTransform.position).normalized;
        rb.linearVelocity = retreatDir * boss.runUpSpeed;

        if (triggerCalled)
        {
            boss.stateMachine.ChangeState(boss.dashAttack);
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.SetZeroVelocity();
    }
}
