using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("HP Settings")]
    public int hp;
    public int maxHp;
    public Slider hpSlider;
    public Animator hpAnimator; 
    public GameObject lowHpScreen;

    [Header("Soul and Geo Settings")]
    public int soul;
    public int geo;
    public Text soulText;
    public Text geoText;

    private void Start()
    {
        UpdateHpUI();
    }

    public void UpdateHp(int newHp)
    {
        hp = Mathf.Clamp(newHp, 0, maxHp);
        UpdateHpUI();

        if (hp <= maxHp * 0.2f)
        {
            hpAnimator.SetTrigger("LowHp");
            lowHpScreen.SetActive(true);
        }
        else
        {
            lowHpScreen.SetActive(false);
        }

        if (hp <= 0)
        {
            hpAnimator.SetTrigger("Dead");
        }
    }

    private void UpdateHpUI()
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)hp / maxHp;
        }
    }

    public void UpdateSoul(int amount)
    {
        soul += amount;
        if (soulText != null)
        {
            soulText.text = "Soul: " + soul;
        }
    }


    public void UpdateGeo(int amount)
    {
        geo += amount;
        if (geoText != null)
        {
            geoText.text = "Geo: " + geo;
        }
    }
}

