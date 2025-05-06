using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Lift : ShakeObject
{
    [Header("충돌")]
    [SerializeField] private Transform LiftCheck;
    [SerializeField] private Vector2 boxSize = new Vector2(2f, 1f);
    [SerializeField] private LayerMask targetLayer;

    [Header("움직임")]
    [SerializeField] private Transform upperPos;
    [SerializeField] private Transform lowerPos;
    [SerializeField] public float moveTime;

    [Header("무게추")]
    [SerializeField] private Transform liftWeightLeft;
    [SerializeField] private Transform liftWeightRight;
    [SerializeField] private Transform weightUpperPos;
    [SerializeField] private Transform weightLowerPos;

    [Header("바퀴")]
    [SerializeField] private GameObject[] wheels;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("SFX")]
    [SerializeField] private AudioClip activate;
    [SerializeField] private AudioClip arrive;

    private AudioSource audiosource;
    private LiftData liftData = new LiftData();

    private bool canMoveStart = true;
    private bool isArrive = true;
    private Player player;

    private void Awake()
    {
        //player = PlayerManager.instance.player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        audiosource = GetComponent<AudioSource>();
        DataManager.instance.RegisterLift(this);
    }

    private void Start()
    {
        SetLiftPos();
    }

    private void SetLiftPos()
    {
        Vector3 pos = new Vector3();

        if (liftData.isLiftUp)
        {
            // 리프트
            pos = transform.position;
            pos.y = upperPos.position.y;
            transform.position = pos;

            // 왼쪽 무게추
            pos = liftWeightLeft.position;
            pos.y = weightLowerPos.position.y;
            liftWeightLeft.position = pos;

            // 오른쪽 무게추
            pos = liftWeightRight.position;
            pos.y = weightUpperPos.position.y;
            liftWeightRight.position = pos;
        }

        else
        {
            // 리프트
            pos = transform.position;
            pos.y = lowerPos.position.y;
            transform.position = pos;

            // 오른쪽 무게추
            pos = liftWeightLeft.position;
            pos.y = weightUpperPos.position.y;
            liftWeightLeft.position = pos;

            // 왼쪽 무게추
            pos = liftWeightRight.position;
            pos.y = weightLowerPos.position.y;
            liftWeightRight.position = pos;
        }
    }

    private void Update()
    {
        Collider2D collider = Physics2D.OverlapBox(LiftCheck.position, boxSize, 0, targetLayer);
        if (collider != null && collider.gameObject.GetComponent<Player>())
        {
            player.transform.SetParent(transform);
            if (canMoveStart)
            {
                if (canMoveStart)
                {
                    LiftActivate();
                }
            }
        }

        // 도착해서 플레이어 내렸을 경우 : 리프트 다시 움직일 수 있음
        else if (isArrive && !canMoveStart && collider == null)
            canMoveStart = true;

        if (player.transform.parent == transform && /*isArrive && */collider == null)
        {
            player.transform.SetParent(null);
        }

        // 테스트용
        if (Input.GetKeyDown(KeyCode.L))
        {
            MoveLift();
        }
    }

    private void LiftActivate()
    {
        SoundManager.Instance.audioSource.PlayOneShot(activate);

        isArrive = false;
        canMoveStart = false;
        Shake(transform, false);
        Invoke("MoveLift", duration + 0.1f);
    }

    private void MoveLift()
    {
        audiosource.Play();

        if (transform.position.y == upperPos.position.y)
            liftData.isLiftUp = false;
        else if (transform.position.y == lowerPos.position.y)
            liftData.isLiftUp = true;

        SetLiftPos(transform, upperPos.position.y, lowerPos.position.y);
        SetLiftPos(liftWeightLeft, weightUpperPos.position.y, weightLowerPos.position.y);
        SetLiftPos(liftWeightRight, weightUpperPos.position.y, weightLowerPos.position.y);
    }

    private void SetLiftPos(Transform target, float _upperPos, float _lowerPos)
    {
        Vector3 startPoint = new Vector3();
        Vector3 endPoint = new Vector3();

        float midPos = (_upperPos + _lowerPos) / 2f;
        // 위 -> 아래
        if (target.position.y >= midPos)
        {
            startPoint = new Vector3(target.position.x, _upperPos);
            endPoint = new Vector3(target.position.x, _lowerPos);
        }
        // 아래 -> 위
        else
        {
            startPoint = new Vector3(target.position.x, _lowerPos);
            endPoint = new Vector3(target.position.x, _upperPos);
        }

        StartCoroutine(RotateWheel());
        StartCoroutine(MoveLift(target, startPoint, endPoint));
    }

    private IEnumerator MoveLift(Transform target, Vector3 startPoint, Vector3 endPoint)
    {
        float elapsed = 0f;
        while (elapsed <= moveTime)
        {
            target.position = Vector3.Lerp(startPoint, endPoint, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isArrive = true;
        target.position = endPoint;

        audiosource.Stop();
        SoundManager.Instance.audioSource.PlayOneShot(arrive);

        if (target.gameObject.GetComponent<Lift>())
        {
            Shake(target, false);
        }
    }

    private IEnumerator RotateWheel()
    {
        float elapsed = 0f;
        while (elapsed <= moveTime)
        {
            float deltaRotation = rotationSpeed * Time.deltaTime;

            foreach (GameObject wheel in wheels)
            {
                wheel.transform.Rotate(0f, 0f, deltaRotation);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(LiftCheck.position, boxSize);
    }

    public LiftData GetSaveData()
    {
        return liftData;
    }

    public void LoadFromData(LiftData _liftData)
    {
        liftData = _liftData;
    }
}
