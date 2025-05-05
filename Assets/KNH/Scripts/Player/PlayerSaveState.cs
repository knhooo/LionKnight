using UnityEngine;

public class PlayerSaveState : PlayerState
{
    public PlayerSaveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.transform.position = player.IsNearBench().transform.position - new Vector3(0, 0.2f);
        player.rb.bodyType = RigidbodyType2D.Static;
        player.isOnBench = true;
        player.SetHPandMP(player.playerData.maxHp, 0);//체력 회복
        //저장 처리
        DataManager.instance.SaveData();
    }

    public override void Exit()
    {
        base.Exit();
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
