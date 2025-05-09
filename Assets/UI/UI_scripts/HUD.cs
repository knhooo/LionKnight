using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instace { get; private set; }
    public string mapname { get; set; }

    private Player player;

    [Header("Soul Settings")]
    public float soul;
    public float maxSoul;
    public Slider soulSlider;

    [Header("Hp and Geo Settings")]
    public float Hp;
    public float maxHp;

    public GameObject HpPrefab;
    public Transform HpContainer;
    public List<GameObject> HpObjects = new List<GameObject>();

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
        UpdatePlayerData();
        InitializeHpUI();
    }

    private void Update()
    {
        UpdatePlayerData();

        UpdateSoulUI();
        UpdateHp();
        UpdateGeo();
    }

    private void UpdatePlayerData()
    {
        Hp = PlayerManager.instance.player.playerData.hp;
        maxHp = PlayerManager.instance.player.playerData.maxHp;
        soul = PlayerManager.instance.player.playerData.mp;
        maxSoul = PlayerManager.instance.player.playerData.maxMp;
        geo = PlayerManager.instance.player.playerData.money;
    }

    private void InitializeHpUI()
    {
        Debug.Log(maxHp/10);
        foreach (GameObject hpObj in HpObjects)
        {
            Destroy(hpObj);
        }
        HpObjects.Clear();

        for (int i = 0; i < maxHp/10; i++)
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
            HpObjects[i].SetActive(i < Hp/10);
        }
    }
    public void UpdateGeo()
    {
        if (geo < 0) geo = 0;

        if (geoText != null)
        {
            geoText.text = geo.ToString();
        }
    }
}

