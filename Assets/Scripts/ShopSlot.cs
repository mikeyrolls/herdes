/**
 * Single shop slot UI element
 */

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ShopSlot : MonoBehaviour {

    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemDisplayName;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] TextMeshProUGUI itemDisplayTypes;
    [SerializeField] Button buyButton;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject soldOutPanel;

    public ItemSpriteDB itemSpriteDB;
    
    public Item item = new Item();
    int price;

    public event Action<ShopSlot> OnPurchaseClicked;

    private void Awake() {
        buyButton.onClick.AddListener(() => OnPurchaseClicked?.Invoke(this));
    }

    public void Init(ItemName itemName) {

        Debug.Log("initializing shop item");

        item.InitializeFromDB(itemName, itemSpriteDB);

        itemImage.sprite = item.itemSprite;
        itemDisplayName.text = item.nameStr;
        itemDescription.text = item.description;
        itemDisplayTypes.text = item.itemTypes;
        price = item.price;
        buttonText.text = price + "G";
    }

    public void DisableExpensive(int finances) {
        if (finances < price) buyButton.interactable = false;
    }

    public void SetSoldOut() {
        soldOutPanel.SetActive(true);
    }

}
