using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    private SpriteRenderer sp;

    [SerializeField] private GameObject brokenTop;
    [SerializeField] private Sprite sprite;
    [SerializeField] private AudioClip breakSound;

    [SerializeField] private bool isCreateBokenParticles = true;

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

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 3f);
        brokenTop = Instantiate(brokenTop, pos, Quaternion.identity);

        brokenTop.GetComponent<SpriteRenderer>().sortingLayerID = sp.sortingLayerID;
        brokenTop.GetComponent<SpriteRenderer>().sortingOrder = sp.sortingOrder;

        if (!isCreateBokenParticles)
            return;

        for (int i = 0; i < 10; i++)
        {
            GameObject particle = PoolManager.instance.Spawn(PoolType.BrokenParticle, transform.position, Quaternion.identity);
            particle.GetComponent<BrokenParticle>().SetSortingInfo(sp.sortingLayerID, sp.sortingOrder);
        }
    }
}
