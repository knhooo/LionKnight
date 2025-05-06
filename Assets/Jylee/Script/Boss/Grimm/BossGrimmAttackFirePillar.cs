using UnityEngine;

public class BossGrimmAttackFirePillar : BossGrimmState
{
    private float generateDelay;
    private int generateCount;
    private float endDelay;
    public BossGrimmAttackFirePillar(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        generateDelay = boss.firePillarDelay;
        generateCount = boss.firePillarCount;
        endDelay = boss.firePillarEndDelay;

        boss.BossGrimmFirePillarVoice();
    }

    public override void Update()
    {
        base.Update();

        if(generateCount > 0)
        {
            generateDelay -= Time.deltaTime;
            if (generateDelay <= 0)
            {
                generateDelay = boss.firePillarDelay;
                boss.BossGrimmFirePillarGenerate();
                generateCount--;
            }
        }
        else
        {
            endDelay -= Time.deltaTime;
            if(endDelay <= 0)
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
