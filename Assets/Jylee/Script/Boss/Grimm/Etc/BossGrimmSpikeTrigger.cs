using UnityEngine;

public class BossGrimmSpikeTrigger : MonoBehaviour
{
    private Collider2D cr;

    private void Start()
    {
        cr = GetComponent<Collider2D>();
    }

    public void SpikeAttackEnable()
    {
        cr.enabled = true;
    }

    public void SpikeAttackDisable()
    {
        cr.enabled = false;
    }
}
