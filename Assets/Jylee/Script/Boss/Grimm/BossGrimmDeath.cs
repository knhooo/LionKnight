using UnityEngine;

public class BossGrimmDeath : BossGrimmState
{
    private float deathAnimDuration;
    public BossGrimmDeath(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        deathAnimDuration = 4f;
    }

    public override void Update()
    {
        base.Update();
        deathAnimDuration -= Time.deltaTime;

        if(deathAnimDuration <= 0)
        {
            boss.BossDeathTrigger();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
