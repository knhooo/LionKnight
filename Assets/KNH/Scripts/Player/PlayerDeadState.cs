using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        stateTimer = 1.5f;
        player.soundClip.PlayerSoundOneShot(6);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //if (stateTimer < 0)
        //{
        //    stateMachine.ChangeState(player.idleState);
        //    SceneManager.LoadScene("Dirtmouth");
        //}
    }
}
