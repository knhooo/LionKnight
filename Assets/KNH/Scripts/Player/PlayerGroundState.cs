using UnityEngine;
using UnityEngine.UIElements;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (player.IsNearBench() && Input.GetKey(KeyCode.UpArrow))
        {
            player.transform.position = player.IsNearBench().transform.position;
            stateMachine.ChangeState(player.benchState);
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Z) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

        //Attack
        if (Input.GetKeyDown(KeyCode.X))
            stateMachine.ChangeState(player.primaryAttack);

        //UpAttack
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.UpArrow))
            stateMachine.ChangeState(player.upAttack);

        //집중(회복)

        // A키를 계속 누르고 있는 중
        if (Input.GetKey(KeyCode.A))
        {
            if (player.mp > 0 && SkillManager.instance.focus.CanUseSkill())
            {
                stateMachine.ChangeState(player.focusState);
            }
        }
    }

}
