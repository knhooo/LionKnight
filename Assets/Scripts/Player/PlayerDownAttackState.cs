using UnityEngine;

public class PlayerDownAttackState : PlayerState
{
    public PlayerDownAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.anim.speed = 1.5f;

        float attackDir = player.facingDir;
        player.soundClip.PlayerSoundOneShot(20);

        if (xInput != 0)
        {
            attackDir = xInput;
        }
        stateTimer = 0.1f;
        player.attackCheck.localPosition = new Vector3(0, -0.29f, 0);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.1f);
        player.anim.speed = 1;
        player.attackCheck.localPosition = new Vector3(1.1f, 0.578f, 0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
