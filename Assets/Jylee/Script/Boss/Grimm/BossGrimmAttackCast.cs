using UnityEngine;

public class BossGrimmAttackCast : BossGrimmState
{
    private bool isFiring;
    private bool isDone;
    private bool isEvade;
    private bool isEmeTeleport;
    private bool isFirstCancel;

    private int shotCount;

    private float firstShotTime;
    private float secondShotTime;
    private float thirdShotTime;
    private float shotEndTime;

    private int teleportCount;

    public BossGrimmAttackCast(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        isFiring = false;
        isDone = false;
        isEvade = false;
        isEmeTeleport = false;
        isFirstCancel = false;
        shotCount = 0;
        teleportCount = 0;
        firstShotTime = boss.firstShotDelay;
        secondShotTime = boss.secondShotDelay;
        thirdShotTime = boss.thirdShotDelay;
        shotEndTime = boss.shotEndDelay;
    }

    public override void Update()
    {
        base.Update();

        if (!isFirstCancel && triggerCalled)
        {
            triggerCalled = false;
            isFirstCancel = true;
        }

        if(isFirstCancel && triggerCalled && isEmeTeleport && !isFiring && !isDone)
        {
            triggerCalled = false;
            teleportCount++;

            if(teleportCount == 1)
            {
                boss.BossRandomTeleportSelect();
                boss.BossFlip(false);
            }

            if(teleportCount == 3)
            {
                isFiring = true;
                thirdShotTime = 0;
                shotEndTime = boss.shotEndDelay;
            }
        }

        if (!isFiring && !isEvade && !isFirstCancel)
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
            if(shotCount > 1 && shotCount < 3 && !isEmeTeleport)
            {
                // 긴급 텔레포트!
                bool doEmeTeleport = false;
                if (!boss.facingLeft && boss.transform.position.x - boss.emeTeleportDistance < boss.playerTransform.position.x)
                {
                    doEmeTeleport = true;
                    Debug.Log(1);
                }
                else if(boss.facingLeft && boss.transform.position.x + boss.emeTeleportDistance > boss.playerTransform.position.x)
                {
                    doEmeTeleport = true;
                    Debug.Log(2);
                }

                if (doEmeTeleport)
                {
                    isEmeTeleport = true;
                    boss.anim.SetTrigger("IsEmeTeleport");
                    isFiring = false;
                    shotCount = 2;
                }
            }

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
