/**
 * Item logic/data class, accessed from ItemGO
 */

using UnityEngine;

[System.Serializable]
public class Item {
    public string nameStr;
    public int price;
    public int value;
    public ItemType itemType;
    public Sprite itemSprite;
    public ItemName itemName;
    public string description;

    public void InitializeFromDB(ItemName itemName, ItemSpriteDB itemSpriteDB) {
        if (ItemDB.items.TryGetValue(itemName, out var itemData)) {
            this.itemName = itemName;
            nameStr = itemData.displayName;
            value = itemData.value;
            price = itemData.price;
            itemType = itemData.itemType;
            itemSprite = itemSpriteDB.Get(itemName);
            if (itemSprite == null) {
                Debug.LogError($"Item Sprite '{itemName}' not found in database!");
            }
        } else {
            Debug.LogError($"Item '{itemName}' not found in database!");
        }
    }

    public void InitGold(int goldValue, ItemSpriteDB itemSpriteDB) {
        if (goldValue < 10) InitializeFromDB(ItemName.GoldCoin, itemSpriteDB);
        else if (goldValue > 20) InitializeFromDB(ItemName.GoldPile, itemSpriteDB);
        else InitializeFromDB(ItemName.GoldBag, itemSpriteDB);
        value = goldValue;
    }

    public void UseItem() {
        if(itemType == ItemType.Heal) {
            GameManager.Instance.hero.Heal(value);
        }
    }
}