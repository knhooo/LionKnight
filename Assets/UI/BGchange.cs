using UnityEngine;
using UnityEngine.Rendering;

public class BGchange : MonoBehaviour
{
    public GameObject unactBG;
    public GameObject actBG;
    public BossGrimm bossGrimm;

    private void Awake()
    {
        if (bossGrimm != null && bossGrimm.IsBossDead())
        {
            ActivateObjects();
        }
    }

    private void ActivateObjects()
    {
        unactBG.SetActive(false);
        actBG.SetActive(true);

        this.enabled = false;
    }
}
