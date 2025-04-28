using UnityEngine;

public class BossGrimmAttackCapeSpike : BossGrimmState
{
    private float actionDuration;
    private float spikeUpDelay;
    private float spikeUpDuration;
    // private bool isAction;
    private int stateType;
    public BossGrimmAttackCapeSpike(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        spikeUpDelay = 0.5f;
        actionDuration = 1f;
        spikeUpDuration = 0.7f;
        stateType = 1;
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
            boss.BossCapeSpikeEnable();
            stateType = 2;
        }

        if (stateType == 2)
        {
            spikeUpDelay -= Time.deltaTime;
            if(spikeUpDelay <= 0)
            {
                stateType = 3;
                boss.BossCapeSpikeUp();
            }
        }

        if(stateType == 3)
        {
            spikeUpDuration -= Time.deltaTime;
            if (spikeUpDuration <= 0)
            {
                stateType = 4;
                boss.BossCapeSpikeDown();
            }
        }

        if(stateType == 4)
        {
            actionDuration -= Time.deltaTime;
            if (actionDuration <= 0)
            {
                boss.stateMachine.ChangeState(boss.teleportInState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
