using UnityEngine;

public class BossGrimmGreet : BossGrimmState
{
    private bool triggerOn;

    public BossGrimmGreet(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 2f;
        triggerOn = false;
        boss.groundY = boss.transform.position.y;
        boss.BossGrimmGreetSound();
    }

    public override void Update()
    {
        base.Update();

        // 지속 시간만큼 인사
        if (stateTimer <= 0 && !triggerOn)
        {
            triggerOn = true;
            boss.anim.SetTrigger("IsGreetClear");
        }

        if(triggerCalled)
        {
            boss.SetTeleportDelay(boss.attackEndTeleportDelay);
            stateMachine.ChangeState(boss.teleportInState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
