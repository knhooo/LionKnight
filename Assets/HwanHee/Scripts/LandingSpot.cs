using Unity.Cinemachine;
using UnityEngine;

public class LandingSpot : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cineCam;

    [SerializeField] private float shakeAmplitude;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private float shakeDuration;

    [SerializeField] private AudioClip landingSound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.audioSource.PlayOneShot(landingSound);
            cineCam.GetComponent<CameraShake>().ShakeCamera(shakeAmplitude, shakeFrequency, shakeDuration);
            Destroy(gameObject);
        }
    }
}
