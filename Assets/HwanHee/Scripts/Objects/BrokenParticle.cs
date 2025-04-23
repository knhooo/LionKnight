using UnityEngine;

public class BrokenParticle : AddForceObject
{
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[Random.Range(0, 4)];
    }

    public void SetSortingInfo(int sortingLayerID, int sortingOrder)
    {
        sr.sortingLayerID = sortingLayerID;
        sr.sortingOrder = sortingOrder;
    }
}
