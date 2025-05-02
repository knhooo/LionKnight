
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private GameObject[] hpBack;
    private Image[] hpFill;

    public Transform hpParent;
    public GameObject hpBackUI;

    private void Start()
    {
        hpBack = new GameObject[(int)HealthSystem.Instance.MaxTotalHealth];
        hpFill = new Image[(int)HealthSystem.Instance.MaxTotalHealth];

        HealthSystem.Instance.hpUpdateCall += UpdateHeartsHUD;
        makeHpUI();
        UpdateHeartsHUD();
    }

    public void UpdateHeartsHUD() 
    {
        makeHpBackUI();
        HpFill();
    }

    void makeHpBackUI()
    {
        for (int i = 0; i < hpBack.Length; i++) 
        {
            if (i < HealthSystem.Instance.MaxHealth) 
            {
                hpBack[i].SetActive(true);
            }
            else
            {
                hpBack[i].SetActive(false);
            }
        }
    }

    void HpFill()
    {
        for (int i = 0; i < hpFill.Length; i++)
        {
            if (i < HealthSystem.Instance.Health)
            {
                hpFill[i].fillAmount = 1;
            }
            else
            {
                hpFill[i].fillAmount = 0;
            }
        }

        if (HealthSystem.Instance.Health % 1 != 0) 
        {
            int lastPos = Mathf.FloorToInt(HealthSystem.Instance.Health);
            hpFill[lastPos].fillAmount = HealthSystem.Instance.Health % 1;
        }
    }

    void makeHpUI()
    {
        for (int i = 0; i < HealthSystem.Instance.MaxTotalHealth; i++)
        {
            GameObject temp = Instantiate(hpBackUI);
            temp.transform.SetParent(hpParent, false);
            hpBack[i] = temp;
            hpFill[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
}
