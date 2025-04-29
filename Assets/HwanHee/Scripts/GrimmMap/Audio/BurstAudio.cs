using UnityEngine;
using UnityEngine.Audio;

public class BurstAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioResource[] audioResources;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(int i)
    {
        audioSource.resource = audioResources[i];
        audioSource.Play();
    }
}
