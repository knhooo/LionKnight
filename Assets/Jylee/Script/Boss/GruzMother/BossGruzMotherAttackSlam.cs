using UnityEngine;

public class BossGruzMotherAttackSlam : BossGruzMotherState
{
    private float duration;
    private float delay;
    private bool isUp;
    private bool isCrash;

    private int stateType;
    public BossGruzMotherAttackSlam(BossGruzMother _boss, BossGruzMotherStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        duration = boss.slamDuration;
        delay = boss.slamDelay;
        isUp = true;
        stateType = 1;
        isCrash = false;
    }

    public override void Update()
    {
        base.Update();
        duration -= Time.deltaTime;
        delay -= Time.deltaTime;

        if(delay <= 0 && stateType == 1)
        {
            if (isCrash) 
            {
                boss.anim.SetTrigger("IsRepeat");
                isCrash = false;
            }

            Vector2 dashDir = isUp ? Vector2.up : Vector2.down;
            Vector2 horizontalDrift = (boss.facingLeft ? Vector2.left : Vector2.right) * boss.slamXSpeed;
            rb.linearVelocity = dashDir * boss.slamYSpeed + horizontalDrift;

            if (isUp && boss.IsCeilDetected())
            {
                isUp = false;
                delay = boss.slamDelay;
                boss.LandEffGenerate(1);
                boss.anim.SetTrigger("IsCrashUp");
                isCrash = true;
            }
            else if (!isUp && boss.IsGroundDetected())
            {
                isUp = true;
                delay = boss.slamDelay;
                boss.LandEffGenerate(3);
                boss.anim.SetTrigger("IsCrashDown");
                isCrash = true;
            }
            else if (boss.IsWallDetected())
            {
                boss.ForcedFlip();
            }
        }

        if (duration <= 0 && stateType == 1)
        {
            boss.anim.SetTrigger("IsSlamEnd");
            stateType = 2;
        }

        if(stateType == 2 && triggerCalled)
        {
            boss.stateMachine.ChangeState(boss.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
