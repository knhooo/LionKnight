using Unity.Cinemachine;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineCam;

    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private float shakeDuration;

    private bool isShake = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isShake)
        {
            isShake = true;
            cineCam.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude, shakeFrequency, shakeDuration);
        }
    }
}
