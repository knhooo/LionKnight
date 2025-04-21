using UnityEngine;

public class BossGrimmAttackDashUppercut : BossGrimmState
{
    private float triggerTimer;
    private bool attackEnd;
    private bool isDone;
    public BossGrimmAttackDashUppercut(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        triggerTimer = 2f;
        attackEnd = false;
        isDone = false;
    }

    public override void Update()
    {
        base.Update();

        if(attackEnd)
        {
            triggerTimer -= Time.deltaTime;
        }

        if(attackEnd && triggerTimer <= 0)
        {
            isDone = true;
        }

        if (isDone)
        {
            boss.stateMachine.ChangeState(boss.attackSelectState);
            return;
        }

        if (triggerCalled && !attackEnd)
        {
            attackEnd = true;
            // 이펙트 터지면서 사라짐
            boss.GrimmInVanish();
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
