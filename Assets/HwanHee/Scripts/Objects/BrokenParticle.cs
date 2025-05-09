using UnityEngine;

public class BrokenParticle : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer sr;

    protected void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[Random.Range(0, 4)];
    }

    public void SetSortingInfo(int sortingLayerID, int sortingOrder)
    {
        sr.sortingLayerID = sortingLayerID;
        sr.sortingOrder = sortingOrder;
    }
}
