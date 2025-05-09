using UnityEngine;

public class BossGrimmScream : BossGrimmState
{
    private float screamDuration;
    private int stateType;
    public BossGrimmScream(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateType = 1;
        screamDuration = boss.screamDuration;
        boss.isInvincible = true;
    }

    public override void Update()
    {
        base.Update();
        if(stateType == 1)
        {
            stateType = 2;
            boss.anim.SetTrigger("IsScreamAction");
            boss.BossGrimmScream();
            boss.PlayerCanMove(false);
        }
        else if(stateType == 2)
        {
            screamDuration -= Time.deltaTime;
            if(screamDuration <= 0)
            {
                boss.anim.SetTrigger("IsScreamReady");
                stateType = 3;
            }
        }
        else if(stateType == 3)
        {
            stateMachine.ChangeState(boss.teleportInState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.isInvincible = false;
        boss.SetTeleportDelay(boss.attackEndTeleportDelay);
        boss.PlayerCanMove(true);
    }
}
