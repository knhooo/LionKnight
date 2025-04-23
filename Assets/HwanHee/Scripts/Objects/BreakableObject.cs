using Mono.Cecil;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    private SpriteRenderer sp;

    [SerializeField] private GameObject brokenParticle;
    [SerializeField] private GameObject fragment;
    [SerializeField] private Sprite sprite;

    private bool isBroken;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !isBroken)
        {
            isBroken = true;
            Break();
        }
    }

    private void Break()
    {
        sp.sprite = sprite;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f);
        fragment = Instantiate(fragment, pos, Quaternion.identity);

        fragment.GetComponent<SpriteRenderer>().sortingLayerID = sp.sortingLayerID;
        fragment.GetComponent<SpriteRenderer>().sortingOrder = sp.sortingOrder;

        if (brokenParticle == null)
            return;

        for (int i = 0; i < 10; i++)
        {
            GameObject particle = Instantiate(brokenParticle, pos, Quaternion.identity);
            particle.GetComponent<BrokenParticle>().SetSortingInfo(sp.sortingLayerID, sp.sortingOrder);
        }
    }
}
