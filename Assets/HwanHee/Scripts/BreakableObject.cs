using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    private SpriteRenderer sp;

    [SerializeField] private GameObject fragment;
    [SerializeField] private Sprite sprite;

    private float forceValueX = 7f;
    private float forceValueY = 5f;

    private bool isBroken;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !isBroken)
        {
            isBroken = true;
            Break();
        }
    }

    private void Break()
    {
        sp.sprite = sprite;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f);
        fragment = Instantiate(fragment, pos, Quaternion.identity);

        fragment.GetComponent<SpriteRenderer>().sortingLayerID = sp.sortingLayerID;
        fragment.GetComponent<SpriteRenderer>().sortingOrder = sp.sortingOrder;

        AddForce();
    }

    private void AddForce()
    {
        Rigidbody2D rb = fragment.GetComponent<Rigidbody2D>();
        rb.angularDamping = 3f;

        float _forceValue = 0f;
        if (GameManager.instance.player.transform.position.x < transform.position.x)
            _forceValue = forceValueX;
        else
            _forceValue = -forceValueX;
        rb.AddForce(new Vector2(_forceValue, forceValueY), ForceMode2D.Impulse);

        float torque = Random.Range(100f, 150f);
        rb.AddTorque(torque);
    }
}
