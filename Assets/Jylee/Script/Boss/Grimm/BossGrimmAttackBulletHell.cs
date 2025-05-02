using UnityEngine;

public class BossGrimmAttackBulletHell : BossGrimmState
{
    private bool isAction;
    private float actionDuration;
    private float fireCount;
    private float fireDelay;
    private float endDelay;
    public BossGrimmAttackBulletHell(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        actionDuration = 5f;
        isAction = false;
        fireCount = boss.bulletFireCount;
        fireDelay = boss.bulletFireDelay;
        endDelay = boss.bulletFireDelay * 2;

        boss.isInvincible = true;

        boss.BossFlip(true);
    }

    public override void Update()
    {
        base.Update();

        // 준비 끝
        if (triggerCalled && !isAction)
        {
            triggerCalled = false;
            boss.anim.SetTrigger("attackBulletHellAction");
            isAction = true;

            boss.BossBulletHellSoundStartLoop();
        }

        // 액션
        if (isAction)
        {
            fireDelay -= Time.deltaTime;

            if (fireDelay <= 0)
            {
                boss.BossGrimmBulletHellGenerate();
                fireDelay = boss.bulletFireDelay;
                fireCount--;
            }

            if(fireCount == 0)
            {
                isAction = false;
            }
        }

        if (fireCount == 0)
        {
            endDelay -= Time.deltaTime;
            if(endDelay <= 0)
            {
                boss.BossBulletHellSoundStopLoop();
                // 공격 끝
                boss.stateMachine.ChangeState(boss.teleportInState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.isInvincible = false;
    }
}
