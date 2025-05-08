using UnityEngine;

public class PlayerAwakeState : PlayerState
{
    public PlayerAwakeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rb.bodyType = RigidbodyType2D.Static;
        player.isOnBench = true;
    }

    public override void Exit()
    {
        base.Exit();
        PlayerManager.instance.isAwake = false;
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0)
        {
            player.rb.bodyType = RigidbodyType2D.Dynamic;
            player.isOnBench = false;
            stateMachine.ChangeState(player.idleState);
        }

    }
}
