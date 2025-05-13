using UnityEngine;
public class PlayerLookUpState : PlayerState

{
    public PlayerLookUpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
