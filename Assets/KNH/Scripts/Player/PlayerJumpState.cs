using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.jumpStartY = player.transform.position.y;

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
                // 현재 높이 계산
                float currentHeight = player.transform.position.y - player.jumpStartY;
                Debug.Log(currentHeight);

                // 최대 점프 높이 이하일 때만 상승
                if (currentHeight < player.maxJumpHeight)
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
                player.isJumping = false;
            }
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
