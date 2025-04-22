using UnityEngine;

public class PlayerUpAttackState : PlayerState
{
    public PlayerUpAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.anim.speed = 1.5f;

        float attackDir = player.facingDir;
        //AttackCheck 위치 다시 설정해줘야함

        if (xInput != 0)
        {
            attackDir = xInput;
        }
        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.1f);
        player.anim.speed = 1;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
