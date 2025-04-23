using UnityEngine;

public class PlayerDoubleJumpState : PlayerJumpState
{
    public PlayerDoubleJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityY(player.jumpForce);
        player.hasDoubleJumped = true;
        player.isJumping = true;
        player.jumpTimer = player.variableJumpTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.X))
            stateMachine.ChangeState(player.primaryAttack);

        if (rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.airState);

        if (player.IsWallDetected())
        {
            player.stateMachine.ChangeState(player.wallSlide);
        }

        if (player.IsGroundDetected())
        {
            player.hasDoubleJumped = false;
            stateMachine.ChangeState(player.idleState);
        }


    }
}
