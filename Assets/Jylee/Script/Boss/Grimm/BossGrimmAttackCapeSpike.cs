using UnityEngine;

public class BossGrimmAttackCapeSpike : BossGrimmState
{
    private float actionDuration;
    private float upDelay;
    private float upDuration;
    private float soundTiming;
    private float nightmareDelay;
    private int stateType;
    public BossGrimmAttackCapeSpike(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        upDelay = boss.spikeUpDelay;
        actionDuration = boss.spikeActionDuration;
        soundTiming = boss.spikeSoundTiming;
        upDuration = boss.spikeUpDuration;
        nightmareDelay = 0.2f;
        stateType = 1;
    }

    public override void Update()
    {
        base.Update();

        Debug.Log("hear");

        // 준비 끝
        if (triggerCalled && !boss.isNightmare)
        {
            triggerCalled = false;
            // 액션
            boss.anim.SetTrigger("attackCapeSpikeAction");
            boss.BossCapeSpikeEnable();
            stateType = 2;
        }
        else if(boss.isNightmare && stateType == 1)
        {
            nightmareDelay -= Time.deltaTime;
            if(nightmareDelay <= 0)
            {
                boss.BossCapeSpikeEnable();
                stateType = 2;
            }
        }

        if (stateType == 2)
        {
            upDelay -= Time.deltaTime;
            if (upDelay <= 0)
            {
                stateType = 3;
                boss.BossCapeSpikeUp();
            }
        }

        if (stateType == 3)
        {
            soundTiming -= Time.deltaTime;
            if (soundTiming <= 0)
            {
                stateType = 4;
                boss.BossCapeUpSound();
                soundTiming = 0.3f;
            }
        }

        if(stateType == 4)
        {
            upDuration -= Time.deltaTime;
            if (upDuration <= 0)
            {
                stateType = 5;
                boss.BossCapeSpikeDown();
            }
        }

        if (stateType == 5)
        {
            soundTiming -= Time.deltaTime;
            if (soundTiming <= 0)
            {
                stateType = 6;
                boss.BossCapeDownSound();
            }
        }

        if (stateType == 6)
        {
            actionDuration -= Time.deltaTime;
            if (actionDuration <= 0)
            {
                if (boss.isNightmare)
                {
                    boss.stateMachine.ChangeState(boss.attackSelectState);
                }
                else
                {
                    boss.stateMachine.ChangeState(boss.teleportInState);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
