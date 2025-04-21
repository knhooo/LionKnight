using UnityEngine;

public class BossGrimmState
{

    protected BossGrimm boss;
    protected BossGrimmStateMachine stateMachine;
    protected Rigidbody2D rb;

    protected string animBoolName;

    protected bool triggerCalled;

    protected float stateTimer;

    public BossGrimmState(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName)
    {
        this.boss = _boss;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        boss.anim.SetBool(animBoolName, true);
        rb = boss.rb;
        triggerCalled = false;
    }


    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        boss.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
