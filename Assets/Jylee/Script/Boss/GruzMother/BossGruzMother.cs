using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class BossGruzMother : BossBase
{

    public bool isSleep;

    [Header("수치 관련")]
    public float bossSpeed;
    public float bossAttackDelay;
    public int nextAttackType;
    public float xPower;
    public float yPower;
    public bool isUp;
    public float detectDelay;
    public Vector2 cdSize;
    public bool isDead;

    [Header("도움 닫기")]
    public float runUpSpeed;
    public float runUpDuration;

    [Header("대쉬 공격")]
    public float dashSpeed;
    public float dashDuration;
    public float dashReboundSpeed;

    [Header("슬램 공격")]
    public float slamXSpeed;
    public float slamYSpeed;
    public float slamDuration;
    public float slamDelay;

    [Header("죽음 관련")]
    public float dyingTime;
    public float dieForceX;
    public float dieForceY;
    public float dieBoundForce;
    public float deadFirstDelay;
    public float gurgleDelay;
    public int gurgleCount;
    public float burstDelay;
    public GameObject deadEventObj;

    [Header("자폭")]
    public GameObject gruzPrefab;
    public GameObject burstParticle;
    public int gruzSpawnCount;
    public List<GameObject> gruzCheck;

    [Header("이펙트 관련")]
    public GameObject reboundEff;

    public Transform playerTransform;
    public Collider2D attackColl;
    public GameObject doorObj;

    private BossGruzMotherSoundClip soundClip;


    // 적의 상태를 관리하는 상태 머신
    public BossGruzMotherStateMachine stateMachine { get; private set; }

    // 적의 상태 (대기 상태, 이동 상태)
    public BossGruzMotherSleep sleepState { get; private set; }
    public BossGruzMotherAwake awakeState { get; private set; }
    public BossGruzMotherIdle idleState { get; private set; }
    public BossGruzMotherAttackCharge attackCharge { get; private set; }
    public BossGruzMotherAttackDash dashAttack { get; private set; }
    public BossGruzMotherAttackSlam slamAttack { get; private set; }
    public BossGruzMotherDying dyingState { get; private set; }
    public BossGruzMotherDead deadState { get; private set; }

    private void Awake()
    {
        stateMachine = new BossGruzMotherStateMachine();

        sleepState = new BossGruzMotherSleep(this, stateMachine, "IsSleep");
        awakeState = new BossGruzMotherAwake(this, stateMachine, "IsAwake");
        idleState = new BossGruzMotherIdle(this, stateMachine, "IsIdle");
        attackCharge = new BossGruzMotherAttackCharge(this, stateMachine, "IsAttackReady");
        dashAttack = new BossGruzMotherAttackDash(this, stateMachine, "IsDashAttack");
        slamAttack = new BossGruzMotherAttackSlam(this, stateMachine, "IsSlamAttack");
        dyingState = new BossGruzMotherDying(this, stateMachine, "IsDying");
        deadState = new BossGruzMotherDead(this, stateMachine, "IsDead");
    }

    protected override void Start()
    {
        base.Start();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        soundClip = GetComponent<BossGruzMotherSoundClip>();

        // 초기 상태를 대기 상태(sleepState)로 설정
        stateMachine.Initalize(sleepState);

        isSleep = true;
        isDead = false;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (Input.GetKeyDown(KeyCode.E))
        {
            BossTakeDamage(100);
        }
    }

    public void BossStartEvent()
    {
        if(doorObj != null)
        {
            doorObj.GetComponent<GruzMotherDoor>().StartCloseDoor();
        }
    }

    public void BossDyingEvent()
    {
        BossExplodeSound();
        BossDefeatSound();
    }

    public void BossEndEvent()
    {
        if (doorObj != null)
        {
            doorObj.GetComponent<GruzMotherDoor>().StartOpenDoor();
        }
    }

    protected override void BossCheckHealthPoint()
    {
        base.BossCheckHealthPoint();

        if (isSleep)
        {
            isSleep = false;
            stateMachine.ChangeState(awakeState);
        }
        else if(currentHealthPoint <= 0)
        {
            BossFinalHitSound();
            Instantiate(deadEventObj, transform.position, Quaternion.identity);
            stateMachine.ChangeState(idleState);
            attackColl.GetComponent<Collider2D>().enabled = false;
            SetZeroVelocity();
            isInvincible = true;
            isDead = true;
            StartCoroutine("BossDefeatEvent");
        }
    }

    IEnumerator BossDefeatEvent()
    {
        yield return new WaitForSeconds(0.5f);
        stateMachine.ChangeState(dyingState);
    }

    public void ForcedFlip()
    {
        FlipReverse();

        // 벽 감지 뒤집기
        Vector3 wallCheckLocalPos = wallCheck.localPosition;
        wallCheckLocalPos.x *= -1;
        wallCheck.localPosition = wallCheckLocalPos;
    }

    public void BossFlip(bool reverse)
    {
        float gazePos = transform.position.x > playerTransform.position.x ? -1 : 1;
        gazePos = reverse ? gazePos * -1 : gazePos;

        // 좌우 반전 했을경우
        if (FlipController(gazePos, reverse))
        {
            // 벽 감지 뒤집기
            Vector3 wallCheckLocalPos = wallCheck.localPosition;
            wallCheckLocalPos.x *= -1;
            wallCheck.localPosition = wallCheckLocalPos;
        }
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public void BossFloatPowerChange()
    {
        float temp = Random.Range(1f, bossSpeed);
        xPower = facingLeft ? -temp : temp;
        yPower = isUp ? bossSpeed - temp : (bossSpeed - temp) * -1;
    }

    public void LandEffGenerate(int type)
    {
        Vector2 pos;
        Quaternion rot;

        if (type == 1)
        {
            pos = ceilCheck.position;
            rot = Quaternion.Euler(0, 0, 180);
        }
        else if(type == 2)
        {
            pos = wallCheck.position;
            rot = facingLeft ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);
        }
        else
        {
            pos = groundCheck.position;
            rot = Quaternion.identity;
        }

        // 좌
        Instantiate(reboundEff, pos, rot);

        // 우
        GameObject mirroEff = Instantiate(reboundEff, pos, rot);
        Vector3 scale = mirroEff.transform.localScale;
        scale.x *= -1;
        mirroEff.transform.localScale = scale;
    }

    public void SelectGruzAttack()
    {
        nextAttackType = Random.Range(1, 3);
    }

    public void GenerateGruzBaby()
    {

        GameObject eff = Instantiate(burstParticle, transform.position, Quaternion.identity);
        Destroy(eff, 1f);
        float yForce = 0f;
        for(int i = 0; i < gruzSpawnCount; i++)
        {
            Vector2 vc = new Vector2(transform.position.x, transform.position.y + yForce);
            GameObject obj = Instantiate(gruzPrefab, vc, Quaternion.identity);
            gruzCheck.Add(obj);
            if(i % 2 == 0)
            {
                yForce += 1f;
            }
        }
    }

    public bool GruzCheck()
    {
        gruzCheck.RemoveAll(minion => minion == null);
        if (gruzCheck.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BossWallCrashSound()
    {
        soundClip.GruzMotherWallCrash();
    }

    public void BossGurgleSound()
    {
        soundClip.GruzMotherGrugle();
    }

    public void BossBurstSound()
    {
        soundClip.GruzMotherBurst();
    }

    public void BossDefeatSound()
    {
        soundClip.BossDefeat();
    }

    public void BossGushingSound()
    {
        soundClip.BossGushing();
    }

    public void BossExplodeSound()
    {
        soundClip.BossExplode();
    }

    public void BossFinalHitSound()
    {
        soundClip.BossFinalHit();
    }
}
