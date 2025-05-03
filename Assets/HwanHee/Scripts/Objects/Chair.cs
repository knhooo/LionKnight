using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField] private ParticleSystem chairParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chairParticle.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chairParticle.Stop();
        }
    }
}
