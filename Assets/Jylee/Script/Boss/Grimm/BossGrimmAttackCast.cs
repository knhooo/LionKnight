using UnityEngine;

public class BossGrimmAttackCast : BossGrimmState
{
    private bool isFiring;
    private bool isDone;

    private int shotCount;

    private float firstShotTime;
    private float secondShotTime;
    private float thirdShotTime;
    private float shotEndTime;

    public BossGrimmAttackCast(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isFiring = false;
        isDone = false;
        shotCount = 0;
        firstShotTime = boss.firstShotDelay;
        secondShotTime = boss.secondShotDelay;
        thirdShotTime = boss.thirdShotDelay;
        shotEndTime = boss.shotEndDelay;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled && !isDone)
        {
            triggerCalled = false;
            isFiring = true;
        }

        if (isFiring)
        {
            switch (shotCount)
            {
                case 0:
                    firstShotTime -= Time.deltaTime;
                    if (firstShotTime <= 0)
                    {
                        boss.BossFireBatFire();
                        shotCount++;
                    }
                    break;
                case 1:
                    secondShotTime -= Time.deltaTime;
                    if (secondShotTime <= 0)
                    {
                        boss.BossFireBatFire();
                        shotCount++;
                    }
                    break;
                case 2:
                    thirdShotTime -= Time.deltaTime;
                    if (thirdShotTime <= 0)
                    {
                        boss.BossFireBatFire();
                        shotCount++;
                    }
                    break;
                case 3:
                    shotEndTime -= Time.deltaTime;
                    if (shotEndTime <= 0)
                    {
                        shotCount++;
                        boss.anim.SetTrigger("attackCastOff");
                        isFiring = false;
                        isDone = true;
                    }
                    break;
            }
        }

        if (triggerCalled && isDone)
        {
            boss.stateMachine.ChangeState(boss.teleportInState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
