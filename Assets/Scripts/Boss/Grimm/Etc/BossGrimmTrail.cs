using UnityEngine;

public class BossGrimmTrail : MonoBehaviour
{
    public float duration;
    void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
