using UnityEditor;
using UnityEngine;

public class BossGrimmChangeBat : BossGrimmState
{
    private float batDuration;
    private float fusionDuration;

    private int stateType;


    public BossGrimmChangeBat(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        batDuration = 2f;
        fusionDuration = 0.4f;
        stateType = 1;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled && stateType == 1)
        {
            stateType = 2;
            boss.GrimmInVanish();
        }

        if(stateType == 2)
        {
            batDuration -= Time.deltaTime;
            if (batDuration <= 0)
            {
                stateType = 3;
            }
        }

        if(stateType == 3)
        {
            fusionDuration -= Time.deltaTime;
            if(fusionDuration <= 0)
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
    }
}
