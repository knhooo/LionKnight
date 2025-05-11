using UnityEngine;

public class SetSoulCount : MonoBehaviour
{
    [SerializeField] private GameObject soulPrefab;

    public void SetSoulCountUI(float soulCount)
    {
        for (int i = 0; i < soulCount; i++)
        {
            GameObject soul = Instantiate(soulPrefab, transform);
        }
    }

    public void InitializeSoulCount()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
