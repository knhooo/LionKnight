using UnityEngine;

public class ShadowIdleState : ShadowState
{
    public ShadowIdleState(Shadow _shadow, ShadowStateMachine _stateMachine, string _animBoolName) : base(_shadow, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
