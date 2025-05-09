using UnityEditor;
using UnityEngine;

public class BossGrimmChangeBat : BossGrimmState
{
    private float batDuration;
    private float combineDuration;

    private int stateType;


    public BossGrimmChangeBat(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        batDuration = boss.grimmChangeBatDuration;
        combineDuration = boss.grimmCombineTime;
        stateType = 1;
        boss.SetZeroVelocity();
        boss.BossCancelEverything();
        boss.BossRotationZero();
        boss.BossGrimmStunSound();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled && stateType == 1)
        {
            stateType = 2;
            boss.GrimmInVanish();
            boss.BossGrimmSplitBat();
        }

        if(stateType == 2)
        {
            batDuration -= Time.deltaTime;
            if (batDuration <= 0)
            {
                boss.BossGrimmCombineBat();
                stateType = 3;
            }
        }

        if(stateType == 3)
        {
            combineDuration -= Time.deltaTime;
            if(combineDuration <= 0)
            {
                boss.stateMachine.ChangeState(boss.teleportInState);
            }
        }
    }

    public override void Exit()
    {
        boss.GrimmOutVanish();
        base.Exit();
        boss.anim.SetTrigger("IsBatEnd");
        boss.BossGrimmAppearSound();
    }
}
