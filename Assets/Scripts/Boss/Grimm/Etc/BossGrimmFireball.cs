using UnityEngine;

public class BossGrimmFireball : MonoBehaviour
{
    public float phaseTime; // 목표 Y까지 이동할 시간 (0.5초 예시)
    public float moveSpeed; // 이동 후 스피드
    public float bulletDuration; // 지속시간

    private Vector2 moveDirection;
    private float posMoveSpeed;
    private float targetY;
    private bool reachedY = false;
    private bool isNormal = true;
    private int xDirection = 1;

    public void Init(float targetYValue, bool isRight)
    {
        targetY = targetYValue;
        xDirection = isRight ? 1 : -1;

        // 거리 계산
        float distanceY = Mathf.Abs(targetY - transform.position.y);

        // 속도 계산
        float ySpeed = distanceY / phaseTime;

        // x 방향 기본 속도 (조정 가능)
        float xSpeed = xDirection * 5f;

        // 처음에는 목표 y로 향하는 방향
        moveDirection = new Vector2(xSpeed, (targetY > transform.position.y ? 1 : -1) * ySpeed);
        moveDirection.Normalize();

        posMoveSpeed = Mathf.Sqrt(xSpeed * xSpeed + ySpeed * ySpeed);

        isNormal = false;
    }

    void Update()
    {
        bulletDuration -= Time.deltaTime;

        if(bulletDuration <= 0)
        {
            Destroy(gameObject);
        }

        if (!reachedY && !isNormal)
        {
            if (Mathf.Abs(transform.position.y - targetY) < 0.1f)
            {
                reachedY = true;
                moveDirection = new Vector2(xDirection, 0f);
                posMoveSpeed = moveSpeed;
            }
        }

        transform.Translate(moveDirection * posMoveSpeed * Time.deltaTime);
    }
}
