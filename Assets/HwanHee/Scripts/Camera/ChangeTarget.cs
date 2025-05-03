using Unity.Cinemachine;
using UnityEngine;

public class ChangeTarget : MonoBehaviour
{
    private bool isTargetChange = false;
    [SerializeField] CinemachineCamera cineCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTargetChange)
        {
            isTargetChange = true;

            cineCam.Follow = collision.transform;
            cineCam.LookAt = collision.transform;
        }
    }
}
