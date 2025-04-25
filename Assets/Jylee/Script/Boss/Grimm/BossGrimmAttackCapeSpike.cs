using UnityEngine;

public class BossGrimmAttackCapeSpike : BossGrimmState
{
    private float actionDuration;
    private bool isAction;
    public BossGrimmAttackCapeSpike(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        actionDuration = 1f;
        isAction = false;
    }

    public override void Update()
    {
        base.Update();

        // 준비 끝
        if (triggerCalled)
        {
            triggerCalled = false;
            // 액션
            boss.anim.SetTrigger("attackCapeSpikeAction");
            isAction = true;
        }

        if (isAction)
        {
            actionDuration -= Time.deltaTime;
        }

        if(actionDuration <= 0)
        {
            boss.stateMachine.ChangeState(boss.teleportInState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
