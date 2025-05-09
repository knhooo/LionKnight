using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIname;

public class ShopManage : MonoBehaviour
{
    public GameObject itemTemplate;
    public Transform contentParent;
    public Text geoText;
    private int playerCoins;
    private Transform previousItem;

    public List<ShopItem> itemsForSale = new List<ShopItem>();
    public List<Achievement> achievements = new List<Achievement>();

    private int curIndex = 0;

    private void Start()
    {
        ShopList();
        UpdateGeoUI();
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

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            ScrollItem(-1);
        }
        else if (scroll < 0f)
        {
            ScrollItem(1);
        }
    }

    private void ShopList()
    {
        foreach (var item in itemsForSale)
        {
            GameObject newItem = Instantiate(itemTemplate, contentParent);
            Text itemText = newItem.transform.GetChild(0).GetComponent<Text>();
            Button buyButton = newItem.GetComponent<Button>();

            if (itemText != null)
            {
                itemText.text = $"{item.itemName}\nPrice: {item.itemPrice}";
            }

            if (buyButton != null)
            {
                buyButton.onClick.AddListener(() => BuyItem(item));
            }
        }
    }

    public void BuyItem(ShopItem item)
    {
        if (PlayerPrefs.HasKey("Money") && PlayerPrefs.GetInt("Money") >= item.itemPrice)
        {
            Buying(item.itemPrice);
            UpdateGeoUI();
        }
    }

    public void BuyItemByIndex(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= itemsForSale.Count) return;

        ShopItem item = itemsForSale[itemIndex];
        BuyItem(item);

        CheckAchievement(itemIndex);
    }

    private void Buying (int amount)
    {
        int currentMoney = PlayerPrefs.GetInt("Money");
        PlayerPrefs.SetInt("Money", currentMoney - amount);
    }

    private void UpdateGeoUI()
    {
        if (geoText != null)
        {
            geoText.text = $"{playerCoins}";
        }
    }

    void CheckAchievement(int itemIndex)
    {
        if (itemIndex == 0 && !achievements[0].isCompleted)
        {
            achievements[0].isCompleted = true;
        }
    }

    private void ScrollItem(int direction)
    {
        if (itemsForSale.Count == 0) return;

        curIndex += direction;

        if (curIndex < 0)
        {
            curIndex = itemsForSale.Count - 1;
        }
        else if (curIndex >= itemsForSale.Count)
        {
            curIndex = 0;
        }

        HighlightCurrentItem();
    }

    private void HighlightCurrentItem()
    {
        ResetPreviousItemHighlight();
        SetCurrentItemHighlight();
    }

    private void ResetPreviousItemHighlight()
    {
        if (previousItem == null) return;

        Animator prevAnimator = previousItem.GetComponent<Animator>();
        if (prevAnimator != null)
        {
            prevAnimator.SetTrigger("Normal");
        }
    }

    private void SetCurrentItemHighlight()
    {
        Transform curItem = contentParent.GetChild(curIndex);
        Animator currAnimator = curItem.GetComponent<Animator>();
        if (currAnimator != null)
        {
            currAnimator.SetTrigger("Highlight");
        }

        previousItem = curItem;
    }
}