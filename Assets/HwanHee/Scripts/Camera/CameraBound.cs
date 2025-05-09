using Unity.Cinemachine;
using UnityEngine;

public class CameraBound : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;

    [SerializeField] private Collider2D GruzToForgottenCameraBound;

    private bool isLoadCamera = false;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        Invoke("SetCameraBound", 0.1f);
    }

    private void SetCameraBound()
    {
        PlayerData playerData = PlayerManager.instance.player.playerData;
        if (playerData.fromSceneName == "GruzMother")
        {
            CinemachineConfiner2D confiner = cinemachineCamera.GetComponent<CinemachineConfiner2D>();
            confiner.BoundingShape2D = GruzToForgottenCameraBound;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
