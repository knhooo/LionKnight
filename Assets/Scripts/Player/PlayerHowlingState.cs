using UnityEngine;

public class PlayerHowlingState : PlayerState
{
    public PlayerHowlingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        stateTimer = player.howlingDuration;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
