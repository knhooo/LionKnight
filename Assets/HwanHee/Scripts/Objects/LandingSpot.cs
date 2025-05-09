using Unity.Cinemachine;
using UnityEngine;

public class LandingSpot : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineCam;

    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private float shakeDuration;

    private void Start()
    {
        if (PlayerManager.instance.player.playerData.fromSceneName == "GruzMother")
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cineCam.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude, shakeFrequency, shakeDuration);
            Destroy(gameObject);
        }
    }
}
