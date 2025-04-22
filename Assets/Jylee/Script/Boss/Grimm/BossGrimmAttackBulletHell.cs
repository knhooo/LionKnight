using UnityEngine;

public class BossGrimmAttackBulletHell : BossGrimmState
{
    private bool isAction;
    private float actionDuration;
    public BossGrimmAttackBulletHell(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        actionDuration = 5f;
        isAction = false;
    }

    public override void Update()
    {
        base.Update();

        // 준비 끝
        if (triggerCalled && !isAction)
        {
            boss.anim.SetTrigger("attackBulletHellAction");
            isAction = true;
        }

        // 액션
        if (isAction)
        {
            // 지속시간 만큼 진행
            actionDuration -= Time.deltaTime;
            if(actionDuration <= 0)
            {
                // 공격 끝
                boss.stateMachine.ChangeState(boss.teleportInState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
