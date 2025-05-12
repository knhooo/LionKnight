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

        // 가시 공격 시작
        if (stateType == 2)
        {
            upDelay -= Time.deltaTime;
            if (upDelay <= 0)
            {
                stateType = 3;
                boss.BossCapeSpikeUp();
            }
        }

        // 사운드
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

        // 가시 공격 끝
        if(stateType == 4)
        {
            upDuration -= Time.deltaTime;
            if (upDuration <= 0)
            {
                stateType = 5;
                boss.BossCapeSpikeDown();
            }
        }

        // 사운드
        if (stateType == 5)
        {
            soundTiming -= Time.deltaTime;
            if (soundTiming <= 0)
            {
                stateType = 6;
                boss.BossCapeDownSound();
            }
        }

        // 일반 -> 텔레포트, 악몽의 왕 -> 공격 선택
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
