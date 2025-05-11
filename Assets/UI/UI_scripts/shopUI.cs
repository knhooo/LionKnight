using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class shopUI : MonoBehaviour
{
    private PlayerData player;
    [SerializeField] private GameObject purchase;
    [SerializeField] private GameObject shopObj;

    [Header("아이템 정보")]
    public GameObject[] items;
    public string[] iname;
    public int[] iprice;
    public string[] idescription;

    [Header("구매 확인")]
    public Text shoptext;
    public Text itemstext;
    public string[] icheck;

    private bool purchaseCheck = false;
    private bool shopActive = false;
    private int curIndex = 0;

    private void Start()
    {
        shopObj.SetActive(shopActive);
    }
    private void Update()
    {
        Listscroll();

        if (Input.GetKeyDown(KeyCode.Escape) && shopActive == true)
        {
            shopObj.SetActive(false);
        }
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
    public void BttSelect() => purchase.gameObject.SetActive(true);
    public void BttYes() => iBuying(curIndex);
    public void BttNo() => purchase.gameObject.SetActive(false);
    public void iBuying(int inum)
    {
        int price = iprice[inum];

        itemstext.text = idescription[inum];
        if (price > player.money)
        {
            StopCoroutine(ShopTextUI());
            StartCoroutine(ShopTextUI());
            return;
        }
        else
        {
            player.money -= price;
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
