using System.Collections;
using UnityEngine;

public class MoveFog : MonoBehaviour
{
    private float moveSpeed = 2f;
    private void Start()
    {
        StartCoroutine(MoveLeft());
    }

    private IEnumerator MoveLeft()
    {
        Vector3 target = new Vector3(-33, transform.position.y);

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
