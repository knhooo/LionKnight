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

        if (!player.isDialog)
        {
            //Bench
            if (player.IsNearBench() && Input.GetKey(KeyCode.UpArrow))
            {
                if (PlayerManager.instance.isAwake)
                {
                    stateMachine.ChangeState(player.awakeState);//처음 시작할 때
                }
                else
                {
                    player.soundClip.PlayerSoundOneShot(0);
                    stateMachine.ChangeState(player.saveState);//중간 저장할 때
                }
            }

            //Jump
            if (Input.GetKeyDown(KeyCode.Z) && player.IsGroundDetected())
                stateMachine.ChangeState(player.jumpState);

            if (Input.GetKeyDown(KeyCode.UpArrow) && player.IsNearBench() == null)
            {
                if (Input.GetKey(KeyCode.A))
                    stateMachine.ChangeState(player.howlingState);
                stateMachine.ChangeState(player.lookUpState);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                stateMachine.ChangeState(player.lookDownState);
            }
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

                if (aKeyHoldTime >= 0.2f && player.playerData.mp >= player.playerData.soul_cost)
                {
                    isAHolding = false;
                    stateMachine.ChangeState(player.focusState);
                }
            }

            if (isAHolding && Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (player.playerData.mp >= player.playerData.soul_cost)
                    stateMachine.ChangeState(player.howlingState);
            }

            // A 키를 뗐을 때
            if (isAHolding && Input.GetKeyUp(KeyCode.A))
            {
                isAHolding = false;

                if (player.playerData.mp >= player.playerData.soul_cost)
                {
                    stateMachine.ChangeState(player.spiritState);
                }
            }
        }
    }
}
