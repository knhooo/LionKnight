using UnityEngine;

public class BossGruzMotherDying : BossGruzMotherState
{
    private float dyingDuration;
    private int stateType;
    private bool hasBounced;
    public BossGruzMotherDying(BossGruzMother _boss, BossGruzMotherStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        dyingDuration = boss.dyingTime;
        stateType = 1;
        hasBounced = false;
        boss.BossGushingSound();
        if (boss.doorObj != null)
        {
            BGMManager.instance.StopBGMFadeOut();
        }
    }

    public override void Update()
    {
        base.Update();

        dyingDuration -= Time.deltaTime;
        if(dyingDuration <= 0 && stateType == 1)
        {
            stateType = 2;
            boss.anim.SetBool("IsFail", true);
            boss.rb.bodyType = RigidbodyType2D.Dynamic;
            boss.rb.gravityScale = 1f;
            boss.rb.linearVelocity = new Vector2(boss.facingLeft ? boss.dieForceX : -boss.dieForceX, boss.dieForceY);

            boss.BossDyingEvent();

            dyingDuration = 0.2f;
        }

        if(stateType == 2 && boss.IsGroundDetected() && dyingDuration <= 0)
        {
            if (!hasBounced)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, boss.dieBoundForce);
                hasBounced = true;
                dyingDuration = 0.2f;
            }
            else
            {
                boss.SetZeroVelocity();
                rb.bodyType = RigidbodyType2D.Kinematic;
                boss.anim.SetTrigger("IsGroundLand");
                stateType = 3;
            }
        }

        if (stateType == 3 && triggerCalled)
        {
            boss.stateMachine.ChangeState(boss.deadState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.anim.SetBool("IsFail", false);
    }
}
