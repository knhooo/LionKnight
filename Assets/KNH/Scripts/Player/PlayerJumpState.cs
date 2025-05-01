using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocityY(player.jumpForce);
        player.isJumping = true;
        player.jumpTimer = player.variableJumpTime;
        player.soundClip.PlayerSoundOneShot(8);
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.Z) && player.isJumping)
        {
            if (player.jumpTimer > 0)
            {
                player.SetVelocity(xInput * player.moveSpeed, player.jumpForce);
                player.jumpTimer -= Time.deltaTime;
            }
            else
            {
                player.isJumping = false;
            }
        }
        else
        {
            player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocityY); 
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            player.isJumping = false;
        }

        //Up Attack
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.UpArrow))
        {
            if (!player.hasAirAttacked)
            {
                player.hasAirAttacked = true;
                stateMachine.ChangeState(player.upAttack);
            }
        }
        //Down Attck
        else if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.DownArrow))
        {
            if (!player.hasAirAttacked)
            {
                player.hasAirAttacked = true;
                stateMachine.ChangeState(player.downAttack);
            }
        }
        //Attack
        else if (Input.GetKeyDown(KeyCode.X) && !player.hasAirAttacked)
        {
            player.hasAirAttacked = true;
            stateMachine.ChangeState(player.primaryAttack);
        }



        if (rb.linearVelocityY < 0 )
            stateMachine.ChangeState(player.airState);

        //DoubleJump
        if (!player.IsGroundDetected() && Input.GetKeyDown(KeyCode.Z) && player.canDoubleJump && !player.hasDoubleJumped)
        {
            stateMachine.ChangeState(player.doubleJumpState);
        }

    }
}
