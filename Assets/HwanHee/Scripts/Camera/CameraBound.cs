using Unity.Cinemachine;
using UnityEngine;

public class CameraBound : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;

    [SerializeField] private Collider2D GruzToForgottenCameraBound;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        if (PlayerManager.instance.player.playerData.fromSceneName == "GruzMother")
            cinemachineCamera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = GruzToForgottenCameraBound;
    }
}
