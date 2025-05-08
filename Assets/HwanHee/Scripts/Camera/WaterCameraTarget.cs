using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterCameraTarget : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineCam;
    [SerializeField] private Transform waterCameraTarget;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (cineCam == null)
            {
                Debug.Log("CinemachineCamera 없음");
                return;
            }
            cineCam.Follow = waterCameraTarget.transform;
            cineCam.LookAt = waterCameraTarget.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (cineCam == null)
            {
                Debug.Log("CinemachineCamera 없음");
                return;
            }

            Player player = PlayerManager.instance.player;
            cineCam.Follow = player.transform;
            cineCam.LookAt = player.transform;
        }
    }
}
