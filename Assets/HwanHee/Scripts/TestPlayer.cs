using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical, 0f).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
