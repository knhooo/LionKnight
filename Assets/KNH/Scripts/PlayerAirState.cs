using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (Input.GetKeyDown(KeyCode.X))
            stateMachine.ChangeState(player.primaryAttack);

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlide);
            return;
        }
        if (player.IsGroundDetected())
        {
            player.anim.SetBool("DoubleJump", false);
            stateMachine.ChangeState(player.idleState);
        }

        //더블점프
        if (!player.IsGroundDetected() && Input.GetKeyDown(KeyCode.Z) && player.canDoubleJump && !player.hasDoubleJumped)
        {
            stateMachine.ChangeState(player.jumpState);
            player.anim.SetBool("DoubleJump", true);
            player.SetVelocityY(player.jumpForce);
            player.hasDoubleJumped = true;
            player.isJumping = true;
            player.jumpTimer = player.variableJumpTime;
        }
        else if (player.IsGroundDetected())
        {
            player.hasDoubleJumped = false;
        }


        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocityY);
    }

}
