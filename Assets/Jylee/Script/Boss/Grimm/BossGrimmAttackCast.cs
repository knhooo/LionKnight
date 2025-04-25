using UnityEngine;

public class BossGrimmAttackCast : BossGrimmState
{
    private bool isFiring;
    private bool isDone;
    private bool isEvade;

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
        isEvade = false;
        shotCount = 0;
        firstShotTime = boss.firstShotDelay;
        secondShotTime = boss.secondShotDelay;
        thirdShotTime = boss.thirdShotDelay;
        shotEndTime = boss.shotEndDelay;
    }

    public override void Update()
    {
        base.Update();

        if (!isFiring && !isEvade)
        {
            bool doEvade = false;
            if(!boss.facingLeft && boss.transform.position.x - boss.evadeDistance < boss.playerTransform.position.x)
            {
                // 오른쪽 이동
                doEvade = true;
            }
            else if(boss.facingLeft && boss.transform.position.x + boss.evadeDistance > boss.playerTransform.position.x)
            { 
                // 왼쪽 이동
                doEvade = true;
            }

            if (doEvade)
            {
                isEvade = true;
                boss.anim.SetTrigger("IsEvade");

                // 좌우 정하기
                float moveDistance = boss.facingLeft ? boss.duDashDistance : -boss.duDashDistance;
                Vector2 direction = (new Vector3(boss.transform.position.x + moveDistance, boss.transform.position.y) - boss.transform.position).normalized;

                boss.rb.linearVelocity = direction * -boss.evadeSpeed;
            }
        }

        if (triggerCalled && !isDone)
        {
            triggerCalled = false;
            isFiring = true;
            isEvade = true;
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
