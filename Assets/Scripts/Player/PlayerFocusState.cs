using UnityEngine;

public class PlayerFocusState : PlayerState
{

    public PlayerFocusState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.focusTimer = 0f;
        player.isFocusing = true;
        player.soundClip.audioSources[2].Play();
        player.SetZeroVelocity(); // 집중할 때 움직이지 않게
    }

    public override void Exit()
    {
        base.Exit();
        player.isFocusing = false;
        player.focusTimer = 0f;
        player.soundClip.audioSources[2].Stop();
    }

    public override void Update()
    {
        base.Update();
        player.focusTimer += Time.deltaTime;
        if (player.focusTimer >= player.requiredFocusTime)
        {
            stateMachine.ChangeState(player.idleState);
            player.isFocusing = false;
        }
        // 중간에 키를 놓으면 실패
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (player.isFocusing && player.focusTimer < player.requiredFocusTime)
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
