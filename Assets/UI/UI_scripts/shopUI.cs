using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class shopUI : MonoBehaviour
{
    private PlayerData playerData;
    [SerializeField] private GameObject purchase;
    [SerializeField] private GameObject shopObj;

    [Header("아이템 정보")]
    public GameObject[] items;
    public int[] iprice;
    public string[] idescription;

    [Header("구매 확인")]
    public Text shoptext;
    public Text itemstext;
    public string[] icheck;

    private bool purchaseCheck = false;
    private int curIndex = 0;


    private void OnEnable()
    {
        LoadShopData();
        LoadShopItem();

        shopObj.SetActive(true);
        purchase.SetActive(false);
    }

    private static void LoadShopData()
    {
        string fullPath = Path.Combine(DataManager.instance.path, DataManager.instance.shopSaveFileName);
        string data = File.ReadAllText(fullPath);
        DataManager.instance.shopData = JsonUtility.FromJson<ShopData>(data);

    }
    private void LoadShopItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (DataManager.instance.shopData.isSolds[i] == true)
            {
                items[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        Listscroll();
    }

    private void Listscroll()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ScrollItem(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ScrollItem(1);
        }
    }
    private void ScrollItem(int dir)
    {
        int itemCount = items.Length;

        curIndex += dir;
        if (curIndex < 0)
        {
            curIndex = itemCount - 1;
        }
        else if (curIndex >= itemCount)
        {
            curIndex = 0;
        }
    }
    public void BttSelect(int inum)
    {
        curIndex = inum;

        purchase.gameObject.SetActive(true);
        itemstext.text = idescription[inum];
    }

    public void BttYes()
    {
        purchase.gameObject.SetActive(false);
        iBuying(curIndex);
    }

    public void BttNo() => purchase.gameObject.SetActive(false);

    public void iBuying(int inum)
    {
        int price = iprice[inum];

        if (playerData == null)
            playerData = PlayerManager.instance.player.playerData;

        if (price > playerData.money)
        {
            StopCoroutine(ShopTextUI());
            StartCoroutine(ShopTextUI());
            return;
        }
        else
        {
            DataManager.instance.shopData.isSolds[inum] = true;

            items[inum].SetActive(false);
            DataManager.instance.SaveShop();

            switch (inum)
            {
                // 영혼 포획자
                case 0:
                    playerData.soul_amount += 5;
                    break;
                // 주술사의 돌
                case 1:
                    playerData.spell_Damage += 10;
                    break;
                // 주문 회오리
                case 2:
                    playerData.soul_cost = 24;
                    break;
                // 대시 마스터
                case 3:
                    playerData.dash_coolTime = 0.3f;
                    break;
                // 불멸의 힘
                case 4:
                    playerData.attack_Damage = 15;
                    break;
            }

            playerData.money -= price;
            purchaseCheck = true;
            //Instantiate(items[inum], inventory.position);
        }
    }

    IEnumerator ShopTextUI()
    {
        if (purchaseCheck == false)
        {
            shoptext.text = icheck[1];
            yield return new WaitForSeconds(1f);
            shoptext.text = icheck[0];
        }
        if (purchaseCheck == true)
        {
            shoptext.text = icheck[2];
            yield return new WaitForSeconds(1f);
            shoptext.text = icheck[0];
            purchaseCheck = false;
        }
    }
}
