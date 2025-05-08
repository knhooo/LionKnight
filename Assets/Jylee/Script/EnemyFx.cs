using System.Collections;
using UnityEngine;

public class EnemyFx : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("플래시 FX")]
    [SerializeField] private Material hitMat;
    private Material originMat;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if(sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        originMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(0.2f);

        sr.material = originMat;
    }
}
