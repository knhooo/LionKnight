using UnityEngine;

public class BrokenParticle : AddForceObject
{
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer sr;

    private bool justInstantiated = true;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[Random.Range(0, 4)];
    }

    protected override void OnEnable()
    {
        if (justInstantiated)
        {
            justInstantiated = false;
            return;
        }
        base.OnEnable();
    }

    public void SetSortingInfo(int sortingLayerID, int sortingOrder)
    {
        sr.sortingLayerID = sortingLayerID;
        sr.sortingOrder = sortingOrder;
    }
}
