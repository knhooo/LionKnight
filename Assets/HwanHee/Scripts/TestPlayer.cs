using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * horizontal * moveSpeed * Time.deltaTime);
    }
}
