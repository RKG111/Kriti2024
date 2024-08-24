using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private IShopCustomer shopCustomer;

    private void Awake(){
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start(){
        CreateItemButton(StoreItems.ItemType.Torch, "Torch", StoreItems.GetCost(StoreItems.ItemType.Torch), 0);
        CreateItemButton(StoreItems.ItemType.Gun, "Gun", StoreItems.GetCost(StoreItems.ItemType.Gun), 1);
        //CreateItemButton(StoreItems.ItemType.Bandage, "Bandage", StoreItems.GetCost(StoreItems.ItemType.Bandage), 2);

        Hide();
    }

    private void CreateItemButton(StoreItems.ItemType itemType, string itemName, int itemcost, int positionIndex){
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 30f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight*positionIndex);

        shopItemTransform.Find("NameText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("PriceText").GetComponent<TextMeshProUGUI>().SetText(itemcost.ToString());

        //shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;
        
        shopItemTransform.GetComponent<Button>().onClick.AddListener(() => {
            // Clicked on the shop item button
            TryBuyItem(itemType);
        });

    }

    private void TryBuyItem(StoreItems.ItemType itemType){
        shopCustomer.BoughtItem(itemType);
    }

    public void Show(IShopCustomer shopCustomer){
        this.shopCustomer = shopCustomer;
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    //Put this inside the player inventory logic
    private void BoughtItem(StoreItems.ItemType itemType){
        Debug.Log("Bought Item" + itemType);
    }
}
