using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player.isRidingLift)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        
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
        else if(Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.DownArrow))
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


        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlide);
            return;
        }
        if (player.IsGroundDetected())
        {
            player.soundClip.PlayerSoundOneShot(10);
            stateMachine.ChangeState(player.idleState);
            player.hasDoubleJumped = false;
            player.hasAirAttacked = false;
        }

        //Double Jump
        if (!player.IsGroundDetected() && Input.GetKeyDown(KeyCode.Z) && player.canDoubleJump && !player.hasDoubleJumped)
        {
            stateMachine.ChangeState(player.doubleJumpState);
        }

        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * xInput, rb.linearVelocityY);

        // A 키를 뗐을 때
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (player.playerData.mp >= player.playerData.soul_cost)
            {
                stateMachine.ChangeState(player.spiritState);
            }
        }
    }

}
