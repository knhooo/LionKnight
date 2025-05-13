using UnityEngine;

public class BossGrimmTeleportOut : BossGrimmState
{
    private float capeTimer;

    public BossGrimmTeleportOut(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 악몽의 왕은 망토 공격시 모습을 보이지 않음
        if (boss.nextAttackType == 4 && boss.isNightmare)
        {
            triggerCalled = true;
        }
        else
        {
            boss.GrimmOutVanish();
            boss.BossFlip(false);
        }
    }

    public override void Update()
    {
        base.Update();

        // 정해진 공격에 따른 상태 이동
        if (triggerCalled)
        {
            switch (boss.nextAttackType)
            {
                case 0:
                    boss.stateMachine.ChangeState(boss.bulletHell);
                    break;
                case 1:
                    boss.stateMachine.ChangeState(boss.dashUppercutState);
                    break;
                case 2:
                    boss.stateMachine.ChangeState(boss.castState);
                    break;
                case 3:
                    boss.stateMachine.ChangeState(boss.airDash);
                    break;
                case 4:
                    boss.stateMachine.ChangeState(boss.capeSpike);
                    break;
                case 5:
                    boss.stateMachine.ChangeState(boss.firePillar);
                    break;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
