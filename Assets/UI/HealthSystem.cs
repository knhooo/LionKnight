
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    public Image currentHealthGlobe;
    public Text healthText;
    public float hitPoint = 100f;
    public float maxHitPoint = 100f;

    public float manaPoint = 100f;
    public float maxManaPoint = 100f;

    public bool Regenerate = true;
    public float regen = 0.1f;
    private float timeleft = 0.0f;
    public float regenUpdateInterval = 1f;

    public delegate void hpUpdate();
    public hpUpdate hpUpdateCall;

    #region Sigleton
    private static HealthSystem instance;
    public static HealthSystem Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<HealthSystem>();
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private float hp;
    [SerializeField]
    private float maxHp;
    [SerializeField]
    private float maxTotalHealth;

    public float Health { get { return hp; } }
    public float MaxHealth { get { return maxHp; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }

    private void UpdateGraphics()
    {
        UpdateHealthGlobe();
    }

    public void Heal(float health)
    {
        this.hp += health;
        hitPoint += health;
        if (hitPoint > maxHitPoint)
            hitPoint = maxHitPoint;

        UpdateGraphics();
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        ClampHealth();

        hitPoint -= dmg;
        if (hitPoint < 1)
            hitPoint = 0;

        UpdateGraphics();
        StartCoroutine(PlayerHurts());
    }

    public void AddHealth()
    {
        if (maxHp < maxTotalHealth)
        {
            maxHp += 1;
            hp = maxHp;

            if (hpUpdateCall != null)
                hpUpdateCall.Invoke();
        }
    }
    public void SetMaxHealth(float max)
    {
        maxHitPoint += (int)(maxHitPoint * max / 100);

        UpdateGraphics();
    }
    void ClampHealth()
    {
        hp = Mathf.Clamp(hp, 0, maxHp);

        if (hpUpdateCall != null)
            hpUpdateCall.Invoke();
    }
    void Start()
    {
        UpdateGraphics();
        timeleft = regenUpdateInterval;
    }
    void Update()
    {
        if (Regenerate)
            Regen();
    }

    private void Regen()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0.0)
        {
            Heal(regen);
            RestoreMana(regen);

            UpdateGraphics();

            timeleft = regenUpdateInterval;
        }
    }

    private void UpdateHealthGlobe()
    {
        float ratio = hitPoint / maxHitPoint;
        currentHealthGlobe.rectTransform.localPosition = new Vector3(0, currentHealthGlobe.rectTransform.rect.height * ratio - currentHealthGlobe.rectTransform.rect.height, 0);
        healthText.text = hitPoint.ToString("0") + "/" + maxHitPoint.ToString("0");
    }

    public void UseMana(float Mana)
    {
        manaPoint -= Mana;
        if (manaPoint < 1)
            manaPoint = 0;

        UpdateGraphics();
    }

    public void RestoreMana(float Mana)
    {
        manaPoint += Mana;
        if (manaPoint > maxManaPoint)
            manaPoint = maxManaPoint;

        UpdateGraphics();
    }
    public void SetMaxMana(float max)
    {
        maxManaPoint += (int)(maxManaPoint * max / 100);

        UpdateGraphics();
    }

    IEnumerator PlayerHurts()
    {
        PopupText.Instance.Popup("HURT", 1f, 1f);
        if (hitPoint < 1)
        {
            yield return StartCoroutine(PlayerDied());
        }

        else
            yield return null;
    }

    IEnumerator PlayerDied()
    {
        PopupText.Instance.Popup("You died", 1f, 1f);

        yield return null;
    }
}
