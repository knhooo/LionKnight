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
    }

    public override void Exit()
    {
        base.Exit();
        boss.cd.offset = Vector2.zero;
        boss.GetComponent<BoxCollider2D>().size = boss.cdSize;
        boss.attackColl.enabled = true;
        boss.BossStartEvent();
    }
}
