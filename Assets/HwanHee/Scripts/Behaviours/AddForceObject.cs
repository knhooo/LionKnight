using UnityEngine;
using UnityEngine.UIElements;

public class AddForceObject : MonoBehaviour
{
    protected Rigidbody2D rb;

    [Header("Force")]
    [SerializeField] protected float SpeedX = 3f;
    [SerializeField] protected float SpeedY = 8f;
    [SerializeField] protected bool usePlayerDirection;
    [SerializeField] protected float torque = 50f;
    [SerializeField] protected float directionValue = 0.5f;

    protected Vector2 velocity;

    protected virtual void Awake()
    {
        SpeedX = Random.Range(SpeedX - 1, SpeedX + 1);
        SpeedY = Random.Range(SpeedY - 1, SpeedY + 1);
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        AddForce();
    }

    protected virtual void AddForce()
    {
        velocity.y = SpeedY;

        if (!usePlayerDirection)
        {
            directionValue = Random.Range(-directionValue, directionValue);
            velocity.x = directionValue * SpeedX;
        }
        else
        {
            if (PlayerManager.instance.player.transform.position.x < transform.position.x)
                velocity.x = directionValue * SpeedX;
            else
                velocity.x = -directionValue * SpeedX;
        }

        rb.AddForce(velocity, ForceMode2D.Impulse);

        if (torque != 0)
        {
            torque = Random.Range(torque, torque + 10f);
            rb.AddTorque(torque);
        }
    }
}
