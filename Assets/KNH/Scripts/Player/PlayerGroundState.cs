using UnityEngine;
using UnityEngine.UIElements;

public class PlayerGroundState : PlayerState
{
    private float aKeyHoldTime = 0f;
    private bool isAHolding = false;
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

        //Bench
        if (player.IsNearBench() && Input.GetKey(KeyCode.UpArrow))
        {
            player.transform.position = player.IsNearBench().transform.position;
            player.soundClip.PlayerSoundOneShot(0);
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

        //주문
        if (Input.GetKeyDown(KeyCode.A))
        {
            aKeyHoldTime = 0f;
            isAHolding = true;
        }

        // A 키를 누르고 있는 중
        if (isAHolding && Input.GetKey(KeyCode.A))
        {
            aKeyHoldTime += Time.deltaTime;

            if (aKeyHoldTime >= 0.2f && player.playerData.mp >= 50)
            {
                isAHolding = false;
                stateMachine.ChangeState(player.focusState);
            }
        }

        // A 키를 뗐을 때 
        if (isAHolding && Input.GetKeyUp(KeyCode.A))
        {
            isAHolding = false;

            if (player.playerData.mp >= 50)
            {
                stateMachine.ChangeState(player.spiritState);
            }
        }

    }
}
