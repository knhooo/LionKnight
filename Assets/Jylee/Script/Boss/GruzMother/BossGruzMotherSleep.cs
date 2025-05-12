using UnityEngine;

public class BossGruzMotherSleep : BossGruzMotherState
{
    public BossGruzMotherSleep(BossGruzMother _boss, BossGruzMotherStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        // 한대 맞으면 Idle로 전환
    }

    public override void Exit()
    {
        base.Exit();
        // 히트박스 재조정 및 기본값
        boss.cd.offset = Vector2.zero;
        boss.GetComponent<BoxCollider2D>().size = boss.cdSize;
        boss.attackColl.enabled = true;
        boss.BossStartEvent();
    }
}
