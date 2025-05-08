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
            burstDelay -= Time.deltaTime;
            if(burstDelay <= 0)
            {
                boss.anim.SetTrigger("IsBurst");
                stateType = 4;
            }
        }
        else if(stateType == 4)
        {
            if (triggerCalled)
            {
                triggerCalled = false;
                boss.GenerateGruzBaby();
                stateType = 5;
            }
        }
        else if(stateType == 5)
        {
            if (boss.GruzCheck())
            {
                stateType = 6;
                burstDelay = 1f;
            }
        }
        else if(stateType == 6)
        {
            burstDelay -= Time.deltaTime;
            if (burstDelay <= 0)
            {
                Debug.Log("AAAA");
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
