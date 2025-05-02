using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManage : MonoBehaviour
{
    public GameObject itemTemplate;
    public Transform contentParent;
    public Text coinText;
    public int playerCoins = 100;

    public List<ShopItem> itemsForSale = new List<ShopItem>();

    private int currentIndex = 0;

    private void Start()
    {
        PopulateShop();
        UpdateCoinUI();
    }

    private void Update()
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
    private void PopulateShop()
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
        if (playerCoins >= item.itemPrice)
        {
            playerCoins -= item.itemPrice;
            UpdateCoinUI();
        }
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {playerCoins}";
        }
    }

    private void ScrollItem(int direction)
    {
        if (itemsForSale.Count == 0) return;

        currentIndex += direction;

        if (currentIndex < 0)
        {
            currentIndex = itemsForSale.Count - 1;
        }
        else if (currentIndex >= itemsForSale.Count)
        {
            currentIndex = 0;
        }

        HighlightCurrentItem();
    }

    private void HighlightCurrentItem()
    {
        for (int i = 0; i < contentParent.childCount; i++)
        {
            Transform item = contentParent.GetChild(i);
            Animator itemAnimator = item.GetComponent<Animator>();
            if (itemAnimator != null)
            {
                itemAnimator.ResetTrigger("Highlight");
                itemAnimator.SetTrigger("Normal");
            }
        }

        Transform currentItem = contentParent.GetChild(currentIndex);
        Animator currentItemAnimator = currentItem.GetComponent<Animator>();
        if (currentItemAnimator != null)
        {
            currentItemAnimator.SetTrigger("Highlight");
        }
    }
}