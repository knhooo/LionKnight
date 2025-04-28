using UnityEngine;

public class CameraOffsetChanger : MonoBehaviour
{
    [SerializeField] private CameraMove cameraMove;
    [SerializeField] private float originCameraOffset;
    [SerializeField] private float modifiedCameraOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            cameraMove.ChangeCameraOffset(modifiedCameraOffset);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraMove.ChangeCameraOffset(originCameraOffset);
        }
    }
}
