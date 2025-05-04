using System.Collections;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    //public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    #endregion

    #region Info
    [Header("Stats")]
    public PlayerData playerData = new PlayerData();

    [Header("공격 디테일")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;
    [HideInInspector]
    public bool hasAirAttacked = false;
    public int attackPower = 10;
    public int spiritAttackPower = 20;

    public bool isBusy { get; private set; }
    [Header("이동 정보")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;
    public float variableJumpTime = 0.3f; // 최대 점프 유지 시간
    public float variableJumpMultiplier = 1f;
    [HideInInspector]
    public bool isJumping = false;
    [HideInInspector]
    public float jumpTimer = 0f;
    public bool canDoubleJump = true;
    [HideInInspector]
    public bool hasDoubleJumped = false;

    [Header("대시 정보")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    [Header("집중 정보")]
    public float focusTimer;
    public float requiredFocusTime = 1.5f;
    public bool isFocusing;
    public float spiritDuration;

    [Header("넉백 정보")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] public float knockbackDuration;
    protected bool isKnocked;

    [Header("피격 정보")]
    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private float shakeDuration;
    [SerializeField] private CinemachineCamera cineCam;
    [SerializeField] private GameObject hitCrack;


    [Header("충돌 정보")]
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("그림자 정보")]
    [SerializeField] protected Vector3[] posArr;
    [SerializeField] protected GameObject shadow;

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] public Transform headPos;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsBench;

    [Header("각 씬 시작 위치")]
    [SerializeField] private Transform StagStationToDirtmouth;
    [SerializeField] private Transform StoreToDirtmouth;
    [SerializeField] private Transform ForgottenCrossroadsToDirtmouth;
    [SerializeField] private Transform GrimmToStagStation;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    public bool isOnBench = false;
    public bool isRidingLift = false;
    public bool isInIntro = false;

    private Coroutine flashRoutine;
    public PlayerSoundClip soundClip => GetComponentInParent<PlayerSoundClip>();
    #endregion

    #region States
    // 플레이어의 상태를 관리하는 상태 머신
    public PlayerStateMachine stateMachine { get; private set; }

    // 플레이어의 상태 (대기 상태, 이동 상태)
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDoubleJumpState doubleJumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerUpAttackState upAttack { get; private set; }
    public PlayerDownAttackState downAttack { get; private set; }
    public PlayerBenchState benchState { get; private set; }
    public PlayerFocusState focusState { get; private set; }
    public PlayerSpiritState spiritState { get; private set; }
    public PlayerHitState hitState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerLookUpState lookUpState { get; private set; }
    public PlayerLookDownState lookDownState { get; private set; }
    #endregion

    public PlayerData GetSaveData()
    {
        playerData.fromSceneName = SceneManager.GetActiveScene().name;
        return playerData;
    }

    public void LoadFromData(PlayerData _playerData)
    {
        playerData = _playerData;
        playerData.toSceneName = SceneManager.GetActiveScene().name;

        string loadKeyName = $"{playerData.fromSceneName}To{playerData.toSceneName}";

        if (loadKeyName == "StagStationToDirtmouth")
            transform.position = StagStationToDirtmouth.position;
        else if (loadKeyName == "StoreToDirtmouth")
            transform.position = StoreToDirtmouth.position;
        else if (loadKeyName == "ForgottenCrossroadsToDirtmouth")
            transform.position = ForgottenCrossroadsToDirtmouth.position;
        else if (loadKeyName == "GrimmToStagStation")
            transform.position = GrimmToStagStation.position;
        else
            transform.position = new Vector2(0f, 0f);
    }

    protected virtual void Awake()
    {
        // 상태 머신 인스턴스 생성
        stateMachine = new PlayerStateMachine();
        // 각 상태 인스턴스 생성 (this: 플레이어 객체, stateMachine: 상태 머신, "Idle"/"Move": 상태 이름)
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        doubleJumpState = new PlayerDoubleJumpState(this, stateMachine, "DoubleJump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        upAttack = new PlayerUpAttackState(this, stateMachine, "UpAttack");
        downAttack = new PlayerDownAttackState(this, stateMachine, "DownAttack");
        benchState = new PlayerBenchState(this, stateMachine, "Sitting");
        focusState = new PlayerFocusState(this, stateMachine, "Focus");
        spiritState = new PlayerSpiritState(this, stateMachine, "Spirit");
        hitState = new PlayerHitState(this, stateMachine, "Hit");
        deadState = new PlayerDeadState(this, stateMachine, "Die");
        lookUpState = new PlayerLookUpState(this, stateMachine, "LookUp");
        lookDownState = new PlayerLookDownState(this, stateMachine, "LookDown");
    }

    protected virtual void Start()
    {
        DataManager.instance.RegisterPlayer(this);
        DataManager.instance.LoadData();

        Debug.Log("lastDeathLocation: " + playerData.lastDeathLocation);
        sr = GetComponentInChildren<SpriteRenderer>();
        //fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        playerData.hp = playerData.maxHp;

        // 게임 시작 시 초기 상태를 대기 상태(idleState)로 설정
        StateInit();
        CheckShadow();
        cineCam = FindFirstObjectByType<CinemachineCamera>();
    }

    private void StateInit()
    {
        if (SceneManager.GetActiveScene().name == "Dirtmouth")
        {
            stateMachine.Initialize(benchState);
            transform.position = new Vector3(0, 0.29f, 0);
        }
        else stateMachine.Initialize(idleState);
    }

    private void CheckShadow()
    {
        //최근 죽은 장소에 도착했을 때 그림자 생성
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (playerData.isShadowAlive && sceneIndex != 0 && sceneIndex == playerData.lastDeathLocation)
        {
            Instantiate(shadow, posArr[sceneIndex], Quaternion.identity);
        }
    }

    protected virtual void Update()
    {
        stateMachine.currentState.Update();
        CheckForDashInput();

        if (isInIntro)
        {
            SetZeroVelocity();
            stateMachine.ChangeState(idleState);
        }

        //개발용 키
        //피격
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage();
        }
        //회복
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetHPandMP(playerData.maxHp, playerData.maxMp);
        }
        //즉사
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetHPandMP(-100, 0);
        }
    }

    public void SetVelocityY(float y)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, y);
    }

    public void TakeDamage()
    {
        if (!isKnocked)
        {
            SetHPandMP(-10, 0);
            stateMachine.ChangeState(hitState);
            if (flashRoutine != null)
                StopCoroutine(flashRoutine);
            //깜빡임 효과
            flashRoutine = StartCoroutine(FlashBlack());
            //이펙트
            GameObject obj = Instantiate(hitCrack, transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            //카메라 쉐이크
            if (cineCam.GetComponent<CameraShake>() != null)
                cineCam.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude, shakeFrequency, shakeDuration);
            //넉백
            StartCoroutine("HitKnockBack");
            //시간느려지는효과
            StartCoroutine(HitStop(0.3f, 0.2f));
        }
    }

    IEnumerator HitStop(float duration, float slowTimeScale)
    {
        Time.timeScale = slowTimeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;

        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;

    }

    #region 충돌
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    public virtual Collider2D IsNearBench()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 1f, whatIsBench);
    }


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region 플립
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }


    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();

    }

    #endregion


    #region 속력
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;
        rb.linearVelocity = new Vector2(0, 0);
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;


        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;


        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.C) && SkillManager.instance.dash.CanUseSkill() && !isOnBench)
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }

    }
    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
    private IEnumerator FlashBlack()
    {
        int flashCount = 4;
        float flashDuration = 0.1f;

        for (int i = 0; i < flashCount; i++)
        {
            sr.color = Color.black;
            yield return new WaitForSeconds(flashDuration);
            sr.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
        }

        sr.color = Color.white;
    }

    public void SetHPandMP(float _hp, float _mp)
    {
        playerData.hp += _hp;
        playerData.mp += _mp;

        if (playerData.hp > playerData.maxHp)
            playerData.hp = playerData.maxHp;
        if (playerData.mp > playerData.maxMp)
            playerData.mp = playerData.maxMp;
        if (playerData.hp <= 0) Die();
        if (playerData.mp < 0) playerData.mp = 0;

        Debug.Log("Hp: " + playerData.hp + " MP: " + playerData.mp);
    }

    public void Die()
    {
        if (cineCam.GetComponent<CameraShake>() != null)
            cineCam.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude, shakeFrequency, 1.4f);
        isKnocked = true;//데미지 받지 않음
        Debug.Log("죽음");
        //그림자가 존재하는 상태에서 죽었을 경우
        if (playerData.isShadowAlive)
        {
            playerData.lostMoney = 0;
        }
        else
        {
            //잃은 돈 저장
            playerData.lostMoney = playerData.money;
        }
        //죽은 씬 저장
        playerData.lastDeathLocation = SceneManager.GetActiveScene().buildIndex;
        playerData.isShadowAlive = true;

        //저장 처리
        DataManager.instance.SaveData();
        Debug.Log("씬 저장 " + playerData.lastDeathLocation);
        stateMachine.ChangeState(deadState);
    }
}

