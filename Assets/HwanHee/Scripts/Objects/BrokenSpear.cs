using UnityEngine;

public class BrokenSpear : MonoBehaviour
{
    [SerializeField] private AudioClip[] breakSounds;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            int index = Random.Range(0, breakSounds.Length);
            SoundManager.Instance.audioSource.PlayOneShot(breakSounds[index]);
        }
    }
}
