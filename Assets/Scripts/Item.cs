using UnityEngine;

[System.Serializable]
public class Item {
    public string nameStr;
    public int price;
    public int value;
    public ItemType itemType;
    public Sprite itemSprite;


    public void InitializeFromDB(ItemName itemName, ItemSpriteDatabase spriteSet) {
        if (ItemDB.items.TryGetValue(itemName, out var itemData)) {
            nameStr = itemData.displayName;
            value = itemData.value;
            price = itemData.price;
            itemType = itemData.itemType;
            itemSprite = spriteSet.GetSprite(itemName);
        } else {
            Debug.LogError($"Item '{itemName}' not found in database!");
        }
    }

    public void InitGold(int goldValue, ItemSpriteDatabase spriteSet) {
        if (goldValue < 10) InitializeFromDB(ItemName.GoldCoin, spriteSet);
        else if (goldValue > 20) InitializeFromDB(ItemName.GoldPile, spriteSet);
        else InitializeFromDB(ItemName.GoldBag, spriteSet);
        value = goldValue;
    }

    public void UseItem() {
        if(itemType == ItemType.Heal) {
            GameManager.Instance.hero.Heal(value);
        }
    }
}