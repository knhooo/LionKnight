using UnityEngine;
using UnityEngine.Windows;

public class ShadowState
{
    protected ShadowStateMachine stateMachine;
    protected Shadow shadow;

    protected Rigidbody2D rb;
    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    public ShadowState(Shadow _shadow, ShadowStateMachine _stateMachine, string _animBoolName)
    {
        this.shadow = _shadow;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;

    }

    public virtual void Enter()
    {
        shadow.anim.SetBool(animBoolName, true);
        rb = shadow.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        shadow.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
