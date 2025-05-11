using UnityEngine;

public class BossGrimmDeath : BossGrimmState
{
    private float deathAnimDuration;
    private bool isDone;
    public BossGrimmDeath(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        DataManager.instance.SaveBossDeath();

        base.Enter();
        deathAnimDuration = boss.bossDeadDelay;
        boss.BossCancelEverything();
        boss.BossGrimmDefeat();
        boss.bossBodyPoint.GetComponent<Collider2D>().enabled = false;
        isDone = false;
        boss.isInvincible = true;
    }

    public override void Update()
    {
        base.Update();
        deathAnimDuration -= Time.deltaTime;

        if(deathAnimDuration <= 0 && !isDone)
        {
            boss.BossDeathTrigger();
            isDone = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
