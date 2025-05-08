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
        boss.SelectGruzAttack();
    }

    public override void Update()
    {
        base.Update();

        Vector2 retreatDir = (boss.transform.position - boss.playerTransform.position).normalized;
        rb.linearVelocity = retreatDir * boss.runUpSpeed;

        if (triggerCalled)
        {
            if(boss.nextAttackType == 1)
            {
                boss.stateMachine.ChangeState(boss.dashAttack);
            }
            else if(boss.nextAttackType == 2)
            {
                boss.stateMachine.ChangeState(boss.slamAttack);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.SetZeroVelocity();
    }
}
