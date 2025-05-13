using Unity.Cinemachine;
using UnityEngine;

public class CameraOffsetChanger : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float originCameraOffset;
    [SerializeField] private float modifiedCameraOffset;
    [SerializeField] private Collider2D originCameraBound;
    [SerializeField] private Collider2D newCameraBound;

    private void Awake()
    {
        if (originCameraBound == null)
            originCameraBound = cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (cinemachineCamera == null)
            {
                Debug.Log("CinemachineCamera 없음");
                return;
            }

            cinemachineCamera.GetComponent<CameraMove>().ChangeCameraOffset(modifiedCameraOffset);

            if (newCameraBound != null)
                cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = newCameraBound;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (cinemachineCamera == null)
            {
                Debug.Log("CinemachineCamera 없음");
                return;
            }
            cinemachineCamera.GetComponent<CameraMove>().ChangeCameraOffset(originCameraOffset);

            if (newCameraBound != null)
                cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = originCameraBound;
        }
    }
}
