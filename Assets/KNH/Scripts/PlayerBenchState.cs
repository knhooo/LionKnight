using UnityEngine;

public class PlayerBenchState : PlayerState
{
    public PlayerBenchState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.transform.position += new Vector3(0, -0.218442f, 0);
        player.rb.bodyType = RigidbodyType2D.Static;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(xInput != 0)
        {
            player.rb.bodyType = RigidbodyType2D.Dynamic;
            stateMachine.ChangeState(player.idleState);
        }

    }
}
