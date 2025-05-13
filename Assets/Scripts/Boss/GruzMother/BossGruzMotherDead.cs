using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BossGruzMotherDead : BossGruzMotherState
{
    private float firstDelay;
    private float gurgleDelay;
    private int gurgleCount;
    private float burstDelay;
    private int stateType;
    public BossGruzMotherDead(BossGruzMother _boss, BossGruzMotherStateMachine _stateMachine, string _animBoolName) : base(_boss, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        firstDelay = boss.deadFirstDelay;
        gurgleDelay = boss.gurgleDelay;
        gurgleCount = 0;
        burstDelay = boss.burstDelay;
        stateType = 1;
    }

    public override void Update()
    {
        base.Update();

        // 특정 시간마다 꿀렁임
        if(stateType == 1)
        {
            firstDelay -= Time.deltaTime;
            if (firstDelay <= 0)
            {
                stateType = 2;
                boss.anim.SetTrigger("IsGurgle");
                gurgleCount += 1;
                boss.BossGurgleSound();
            }
        }
        else if (stateType == 2)
        {
            gurgleDelay -= Time.deltaTime;
            if(gurgleDelay <= 0)
            {
                // 횟수만큼 꿀렁이는데 횟수 도달시 폭발 임박
                gurgleDelay = boss.gurgleDelay;
                if (gurgleCount >= boss.gurgleCount)
                {
                    boss.anim.SetTrigger("IsBurstImm");
                    stateType = 3;
                    boss.BossGurgleSound();
                    boss.Invoke("BossBurstSound", 1.2f);
                }
                else
                {
                    boss.anim.SetTrigger("IsGurgle");
                    gurgleCount += 1;
                    boss.BossGurgleSound();
                }
            }
        }
        else if(stateType == 3)
        {
            // 시간이 끝나면 폭발
            burstDelay -= Time.deltaTime;
            if(burstDelay <= 0)
            {
                boss.anim.SetTrigger("IsBurst");
                stateType = 4;
            }
        }
        else if(stateType == 4)
        {
            // 쫄 생성
            if (triggerCalled)
            {
                triggerCalled = false;
                boss.GenerateGruzBaby();
                stateType = 5;
            }
        }
        else if(stateType == 5)
        {
            // 쫄 수 확인
            if (boss.GruzCheck())
            {
                stateType = 6;
                burstDelay = 1f;
            }
        }
        else if(stateType == 6)
        {
            // 문 열림
            burstDelay -= Time.deltaTime;
            if (burstDelay <= 0)
            {
                stateType = 7;
                boss.BossEndEvent();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
