using UnityEngine;

public class AddForceObject : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Force")]
    [SerializeField] private float SpeedX = 3f;
    [SerializeField] private float SpeedY = 8f;
    [SerializeField] private bool usePlayerDirection = true;
    [SerializeField] private float torque = 50f;
    [SerializeField] private float directionValue = 0.5f;

    private Vector2 velocity = new Vector2();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        SpeedX = Random.Range(SpeedX - 1, SpeedX + 1);
        SpeedY = Random.Range(SpeedY - 1, SpeedY + 1);
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        AddForce();
    }

    private void AddForce()
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
