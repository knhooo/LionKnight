using Mono.Cecil;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    private SpriteRenderer sp;

    [SerializeField] private GameObject brokenParticle;
    [SerializeField] private GameObject brokenBase;
    [SerializeField] private Sprite sprite;
    [SerializeField] private AudioClip breakSound;

    private bool isBroken;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    public void Break()
    {
        if (isBroken)
            return;
        isBroken = true;

        SoundManager.Instance.audioSource.PlayOneShot(breakSound);

        CreateBaseAndParticles();
    }

    private void CreateBaseAndParticles()
    {
        sp.sprite = sprite;

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f);
        brokenBase = Instantiate(brokenBase, pos, Quaternion.identity);

        brokenBase.GetComponent<SpriteRenderer>().sortingLayerID = sp.sortingLayerID;
        brokenBase.GetComponent<SpriteRenderer>().sortingOrder = sp.sortingOrder;

        if (brokenParticle == null)
        {
            Debug.LogError("Broken particle 없음");
            return;
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject particle = Instantiate(brokenParticle, pos, Quaternion.identity);
            particle.GetComponent<BrokenParticle>().SetSortingInfo(sp.sortingLayerID, sp.sortingOrder);
        }
    }
}
