using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instace { get; private set; }
    public string mapname { get; set; }

    private Player player;

    [Header("Soul Settings")]
    [Range(0, 10)] public float soul;
    [Range(0, 10)] public float maxSoul;
    public Slider soulSlider;

    [Header("Hp and Geo Settings")]
    [Range(0, 10)] public int Hp;
    [Range(0, 10)] public int maxHp;

    public GameObject HpPrefab;
    public Transform HpContainer;
    private List<GameObject> HpObjects = new List<GameObject>();

    public int geo;
    public Text geoText;

    public void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        InitializeHpUI();
    }

    private void Update()
    {
        UpdateSoulUI();
        UpdateHp();

    }

    private void InitializeHpUI()
    {
        foreach (GameObject hpObj in HpObjects)
        {
            Destroy(hpObj);
        }
        HpObjects.Clear();

        for (int i = 0; i < maxHp; i++)
        {
            AddHpObject();
        }
    }
    private void AddHpObject()
    {
        if (HpPrefab != null && HpContainer != null)
        {
            GameObject newHp = Instantiate(HpPrefab, HpContainer);
            HpObjects.Add(newHp);
        }
    }

    private void RemoveHpObject()
    {
        if (HpObjects.Count > 0)
        {
            GameObject lastHp = HpObjects[HpObjects.Count - 1];
            HpObjects.RemoveAt(HpObjects.Count - 1);
            Destroy(lastHp);
        }
    }

    public void UpdateHpUI(float newMaxHp)
    {
        if (newMaxHp > maxHp)
        {
            for (float i = maxHp; i < newMaxHp; i++)
            {
                AddHpObject();
            }
        }
        else if (newMaxHp < maxHp)
        {
            for (float i = maxHp; i > newMaxHp; i--)
            {
                RemoveHpObject();
            }
        }
        maxHp = (int)newMaxHp;
    }
    private void UpdateSoulUI()
    {
        soul = Mathf.Clamp(soul, 0, maxSoul);
        if (soulSlider != null)
        {
            soulSlider.value = soul / maxSoul;
        }
    }
    public void UpdateHp()
    {

        Hp = Mathf.Clamp(Hp, 0, maxHp);

        for (int i = 0; i < HpObjects.Count; i++)
        {
            HpObjects[i].SetActive(i < Hp);
        }
    }
    public void UpdateGeo(int money)
    {
        geo += money;
        if (geo < 0) geo = 0;

        if (geoText != null)
        {
            geoText.text = player.ToString();
        }
    }
}

