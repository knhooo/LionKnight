using System.Collections;
using UnityEngine;

public class BossGrimmAttackDashUppercut : BossGrimmState
{
    private float triggerTimer;
    private bool attackEnd;
    private bool isDone;



    private int stateType;
    public BossGrimmAttackDashUppercut(BossGrimm _boss, BossGrimmStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        triggerTimer = 2f;
        attackEnd = false;
        isDone = false;
        stateType = 1;
    }

    public override void Update()
    {
        base.Update();

        // 전방 공격
        if(triggerCalled && stateType == 1)
        {
            stateType = 2;
            triggerCalled = false;

            // 좌우 정하기
            float moveDistance = boss.facingLeft ? boss.duDashDistance : -boss.duDashDistance;
            Vector2 direction = (new Vector3(boss.transform.position.x + moveDistance, boss.transform.position.y) - boss.transform.position).normalized;

            // 이동 시작
            boss.rb.linearVelocity = direction * boss.duDashPower;

            // 감속 주기
            boss.StartCoroutine(Decelerate(boss.rb.linearVelocity, 0.2f));
        }

        // 전방 공격 끝
        if (triggerCalled && stateType == 2)
        {
            stateType = 3;
            triggerCalled = false;

            // 감속 종료
            boss.StopCoroutine("Decelerate");
            boss.SetZeroVelocity();
        }

        // 어퍼컷 공격
        if (triggerCalled && stateType == 3)
        {
            float xVel = boss.facingLeft ? boss.duUppercutDistance : -boss.duUppercutDistance;
            float yVel = boss.duUppercutPower;

            rb.linearVelocity = new Vector2(xVel, yVel);

            stateType = 4;
            triggerCalled = false;
        }

        // 어퍼컷 공격 끝
        if (triggerCalled && stateType == 4)
        {
            // 이동 종료 및 위치 고정
            rb.gravityScale = 0;
            boss.SetZeroVelocity();

            // 스파크 생성
            boss.BossGrimmSparkGenerate();


            stateType = 5;
            triggerCalled = false;
        }

        if (triggerCalled && stateType == 5)
        {
            boss.TeleportEffGenerate();
            boss.GrimmInVanish();
            triggerCalled = false;
        }

        if(stateType == 5)
        {
            triggerTimer -= Time.deltaTime;
            if (triggerTimer <= 0)
            {
                boss.stateMachine.ChangeState(boss.attackSelectState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        boss.rb.gravityScale = boss.bossGravity;
    }

    // 감속 코루틴
    private IEnumerator Decelerate(Vector2 initialVelocity, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            Vector2 currentVelocity = Vector2.Lerp(initialVelocity, Vector2.zero, t);
            rb.linearVelocity = currentVelocity;

            time += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
    }
}
