using UnityEngine;

public class BossGrimmAttackCast : BossGrimmState
{
    private bool isFiring;
    private bool isDone;
    private float fireTime;
    public BossGrimmAttackCast(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isFiring = false;
        isDone = false;
        fireTime = 2f;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled && !isFiring && !isDone)
        {
            triggerCalled = false;
            isFiring = true;
        }

        if(triggerCalled && isDone)
        {
            boss.stateMachine.ChangeState(boss.teleportInState);
        }

        if (isFiring)
        {
            fireTime -= Time.deltaTime;
        }

        if(isFiring && fireTime <= 0 && !isDone)
        {
            boss.anim.SetTrigger("attackCastOff");
            isFiring = false;
            isDone = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
