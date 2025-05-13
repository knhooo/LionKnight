using UnityEngine;

public class BossGrimmAttackSelect : BossGrimmState
{
    private float tpOutDelay;

    public BossGrimmAttackSelect(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 공격 선택
        boss.SelectGrimmAttack();
        // 공격 위치 조정
        boss.BossRandomTeleportSelect();

        tpOutDelay = boss.teleportOutDelay;
    }

    public override void Update()
    {
        base.Update();

        tpOutDelay -= Time.deltaTime;

        if (tpOutDelay <= 0)
        {
            // 정해진 일반공격 진행 or 특정 패턴 진행
            boss.stateMachine.ChangeState(boss.teleportOutState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
