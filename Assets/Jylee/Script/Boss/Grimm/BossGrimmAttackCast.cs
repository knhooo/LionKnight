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
    private float fourthShotTime;
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
        fourthShotTime = boss.fourthShotDelay;
        shotEndTime = 0.2f;
    }

    public override void Update()
    {
        base.Update();

        // 첫 시작용 트리거
        if (!isFirstCancel && triggerCalled)
        {
            triggerCalled = false;
            isFirstCancel = true;
        }

        // 
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

        // 공격 시작 전에 플레이어와 가까워지면 하는 회피
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
                boss.BossGrimmEvadeSound();

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
            // 발사 중 플레이어가 보스와의 거리가 가까워지거나 넘어서면 긴급 텔레포트
            if (shotCount > 1 && shotCount < 3 && !isEmeTeleport)
            {
                // 긴급 텔레포트!
                bool doEmeTeleport = false;
                if (!boss.facingLeft && boss.transform.position.x - boss.emeTeleportDistance < boss.playerTransform.position.x)
                {
                    doEmeTeleport = true;
                }
                else if(boss.facingLeft && boss.transform.position.x + boss.emeTeleportDistance > boss.playerTransform.position.x)
                {
                    doEmeTeleport = true;
                }

                if (doEmeTeleport)
                {
                    isEmeTeleport = true;
                    boss.anim.SetTrigger("IsEmeTeleport");
                    isFiring = false;
                    shotCount = 2;
                    if (boss.isNightmare)
                    {
                        fourthShotTime = 0;
                    }
                }
            }

            // 샷 카운트에 따른 발사
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
                        // 악몽의 왕은 위쪽 발사
                        if (boss.isNightmare)
                        {
                            boss.BossFireBatFireUp();
                        }
                        else
                        {
                            boss.BossFireBatFire();
                        }
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
                    // 악몽의 왕이면 4발 발사
                    if (boss.isNightmare)
                    {
                        fourthShotTime -= Time.deltaTime;
                        if (fourthShotTime <= 0)
                        {
                            boss.BossFireBatFireUp();
                            shotCount++;
                        }
                    }
                    else
                    {
                        shotEndTime -= Time.deltaTime;
                        if (shotEndTime <= 0)
                        {
                            shotCount++;
                            boss.anim.SetTrigger("attackCastOff");
                            isFiring = false;
                            isDone = true;
                        }
                    }
                    break;
                case 4:
                    shotEndTime -= Time.deltaTime;
                    if (shotEndTime <= 0)
                    {
                        shotCount++;
                        isFiring = false;
                        isDone = true;
                    }
                    break;
            }
        }

        if ((triggerCalled && isDone) || (boss.isNightmare && isDone))
        {
            boss.stateMachine.ChangeState(boss.waitState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
