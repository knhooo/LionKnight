using UnityEngine;

public class PlayerLookDownState : PlayerState
{
    public PlayerLookDownState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
