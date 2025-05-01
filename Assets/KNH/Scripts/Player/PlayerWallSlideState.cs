using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.soundClip.audioSources[1].Play();
    }

    public override void Exit()
    {
        base.Exit();
        player.soundClip.audioSources[1].Stop();
    }

    public override void Update()
    {
        base.Update();


        if (Input.GetKeyDown(KeyCode.Z))
        {
            stateMachine.ChangeState(player.wallJump);
            return;
        }

        if (xInput != 0 && player.facingDir != xInput)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (yInput < 0)
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY * 0.7f);

        if (!player.IsWallDetected())
            stateMachine.ChangeState(player.airState);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
