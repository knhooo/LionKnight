using System.Collections;
using UnityEngine;

public class Lift : ShakeObject
{
    [Header("충돌")]
    [SerializeField] private Transform LiftCheck;
    [SerializeField] private Vector2 boxSize = new Vector2(2f, 1f);
    [SerializeField] private LayerMask targetLayer;

    [Header("움직임")]
    [SerializeField] private float upperPos = -0.32f;
    [SerializeField] private float lowerPos = -11.52f;
    [SerializeField] public float moveTime;

    [Header("무게추")]
    [SerializeField] private GameObject liftWeightLeft;
    [SerializeField] private GameObject liftWeightRight;
    [SerializeField] private float weightUpperPos = 1.44f;
    [SerializeField] private float weightLowerPos = -4.86f;

    [Header("바퀴")]
    [SerializeField] private GameObject[] wheels;
    [SerializeField] private float rotationSpeed = 360f;

    private bool canMoveStart = true;
    private bool isArrive = true;
    private Player player;

    private void Awake()
    {
        //player = PlayerManager.instance.player;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        Collider2D collider = Physics2D.OverlapBox(LiftCheck.position, boxSize, 0, targetLayer);
        if (collider != null && collider.gameObject.GetComponent<Player>() && canMoveStart)
        {
            player.transform.SetParent(transform);

            isArrive = false;
            canMoveStart = false;
            Shake(transform, false);
            Invoke("MoveLift", duration + 0.1f);
        }

        else if (isArrive && !canMoveStart && collider == null)
            canMoveStart = true;

        if (player.transform.parent == transform && collider == null)
        {
            player.transform.SetParent(null);
        }
    }

    private void MoveLift()
    {
        SetLiftPos(transform, upperPos, lowerPos);
        SetLiftPos(liftWeightLeft.transform, weightUpperPos, weightLowerPos);
        SetLiftPos(liftWeightRight.transform, weightUpperPos, weightLowerPos);
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

        target.position = endPoint;

        isArrive = true;

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
}
