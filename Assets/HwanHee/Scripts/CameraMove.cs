using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float holdKeyTime = 0.5f;
    [SerializeField] private float cameraMoveTime = 0.3f;
    [SerializeField] private float cameraMoveOffset = 2f;

    private CinemachineCamera cineCam;
    private CinemachinePositionComposer positionComposer;

    private bool isCameraMoving = false;
    private float originOffset;

    public bool isCameraAnimationPlay = true;

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
    }

    private void HandleCameraInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            originOffset = positionComposer.TargetOffset.y;
            StartCoroutine(MoveCameraAfterDelay(originOffset - cameraMoveOffset));
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isCameraAnimationPlay = false;
            MoveCameraToOffset(originOffset);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            originOffset = positionComposer.TargetOffset.y;
            StartCoroutine(MoveCameraAfterDelay(originOffset + cameraMoveOffset));
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isCameraAnimationPlay = false;
            MoveCameraToOffset(originOffset);
        }
    }

    private IEnumerator MoveCameraAfterDelay(float targetOffset)
    {
        yield return new WaitForSeconds(holdKeyTime);
        MoveCameraToOffset(targetOffset);
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
            positionComposer.TargetOffset.y = Mathf.Lerp(startOffset, endOffset, elapsed / cameraMoveTime);
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
