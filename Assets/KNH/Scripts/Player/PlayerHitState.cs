using UnityEngine;

public class PlayerHitState : PlayerState
{
    public PlayerHitState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.knockbackDuration;
    }

    public override void Update()
    {
        base.Update();
        if (player.playerData.hp <= 0)
            stateMachine.ChangeState(player.deadState);
        if(stateTimer < 0.5)
        {
            if(player.IsGroundDetected() || player.IsWallDetected())
                stateMachine.ChangeState(player.idleState);
        }
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.linearVelocityY);
    }
}
