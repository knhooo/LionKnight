using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Timers;

enum KeyDirection
{
    Down = -1,
    None,
    Up = 1
}

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float holdKeyTime = 0.5f;
    [SerializeField] private float cameraMoveTime = 0.3f;
    [SerializeField] private float cameraMoveOffset = 2f;

    private CinemachineCamera cineCam;
    private CinemachinePositionComposer positionComposer;

    private bool isCameraMoving = false;
    private float originOffset;
    private float holdKeyElpased = 0f;
    private KeyDirection keyDirection = KeyDirection.None;

    public bool isCameraAnimationPlay = false;

    void Start()
    {
        cineCam = GetComponent<CinemachineCamera>();
        positionComposer = cineCam.GetComponent<CinemachinePositionComposer>();

        if (positionComposer == null)
        {
            Debug.LogError("카메라 없음");
            return;
        }
    }

    void Update()
    {
        if (positionComposer == null || isCameraMoving)
            return;

        HandleCameraInput();

        if (keyDirection != KeyDirection.None)
        {
            holdKeyElpased += Time.deltaTime;
            if (holdKeyElpased >= holdKeyTime)
            {
                holdKeyElpased = 0f;
                MoveCameraToOffset(originOffset + cameraMoveOffset * (int)keyDirection);
            }
        }
    }

    private void HandleCameraInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            keyDirection = KeyDirection.Down;
            originOffset = positionComposer.TargetOffset.y;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            keyDirection = KeyDirection.None;
            isCameraAnimationPlay = false;

            holdKeyElpased = 0f;
            MoveCameraToOffset(originOffset);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            keyDirection = KeyDirection.Up;
            originOffset = positionComposer.TargetOffset.y;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            keyDirection = KeyDirection.None;
            isCameraAnimationPlay = false;

            holdKeyElpased = 0f;
            MoveCameraToOffset(originOffset);
        }
    }

    private void MoveCameraToOffset(float targetOffset)
    {
        if (isCameraMoving)
            return;

        isCameraMoving = true;
        StartCoroutine(MoveCamera(positionComposer.TargetOffset.y, targetOffset));
    }

    private IEnumerator MoveCamera(float startOffset, float endOffset)
    {
        isCameraAnimationPlay = true;
        float elapsed = 0f;

        while (elapsed < cameraMoveTime)
        {
            elapsed += Time.deltaTime;

            float nextY = Mathf.Lerp(startOffset, endOffset, elapsed / cameraMoveTime);

            if (Mathf.Approximately(nextY, positionComposer.TargetOffset.y))
                break;

            yield return null;
        }

        positionComposer.TargetOffset.y = endOffset;
        isCameraMoving = false;
    }

    public void ChangeCameraOffset(float newOffset)
    {
        if (this != null && !isCameraMoving)
        {
            StartCoroutine(MoveCamera(positionComposer.TargetOffset.y, newOffset));
        }
    }
}
