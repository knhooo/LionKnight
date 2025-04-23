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
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Z) && player.isJumping)
        {
            if (player.jumpTimer > 0)
            {
                player.SetVelocityY(player.jumpForce); // 계속 상승
                player.jumpTimer -= Time.deltaTime;
            }
            else
            {
                player.isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            player.isJumping = false;
        }

        //공중 위 공격
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.UpArrow))
        {
            if (!player.hasAirAttacked)
            {
                player.hasAirAttacked = true;
                stateMachine.ChangeState(player.upAttack);
            }
        }
        //공중에서 공격 1회만 가능
        else if (Input.GetKeyDown(KeyCode.X) && !player.hasAirAttacked)
        {
            player.hasAirAttacked = true;
            stateMachine.ChangeState(player.primaryAttack);
        }



        if (rb.linearVelocityY < 0 )
            stateMachine.ChangeState(player.airState);

        //더블점프
        if (!player.IsGroundDetected() && Input.GetKeyDown(KeyCode.Z) && player.canDoubleJump && !player.hasDoubleJumped)
        {
            stateMachine.ChangeState(player.doubleJumpState);
        }

    }
}
