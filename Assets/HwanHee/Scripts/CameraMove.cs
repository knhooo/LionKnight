using UnityEngine;
using Unity.Cinemachine;
using UnityEditor.Search;

public class CameraMove : MonoBehaviour
{
    public CinemachineCamera cineCam;
    public Transform player;
    public float moveSpeed = 1f;
    public float minOffsetY = -2f;
    public float maxOffsetY = 2f;

    private CinemachinePositionComposer positionComposer;

    void Start()
    {
        positionComposer = cineCam.GetComponent<CinemachinePositionComposer>();
        if (positionComposer == null)
        {
            Debug.LogError("카메라 없음");
        }
    }

    void Update()
    {
        if (positionComposer == null) 
            return;

        float inputY = Input.GetAxisRaw("Vertical");
        if (inputY > 0)
            MoveCameraOffset(1);
        else if (inputY < 0)
            MoveCameraOffset(-1);
    }

    void MoveCameraOffset(int direction)
    {
        Vector3 offset = positionComposer.TargetOffset;
        offset.y += direction * moveSpeed * Time.deltaTime;
        offset.y = Mathf.Clamp(offset.y, minOffsetY, maxOffsetY);
        positionComposer.TargetOffset = offset;
    }
}