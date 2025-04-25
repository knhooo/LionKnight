using UnityEngine;

public class PlayerSpiritState : PlayerState
{
    public PlayerSpiritState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        stateTimer = player.spiritDuration;

        SkillManager.instance.spirit.UseSpiritSkill();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
