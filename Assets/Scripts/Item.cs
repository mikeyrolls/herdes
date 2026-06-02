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
            description = itemData.description;
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

    public bool IsConsumable() { 
        return (itemType == ItemType.Heal || itemType == ItemType.Upgrade || itemType == ItemType.Buff);
    }

    public void UseItem() {
        switch(itemType) {
            case ItemType.Heal:
                GameManager.Instance.hero.Heal(value);
                break;
            case ItemType.Upgrade:
                if(itemName == ItemName.HealthUpgrade) GameManager.Instance.hero.IncreaseStatPermanent(value, StatType.MaxHP);
                if(itemName == ItemName.DmgUpgrade) GameManager.Instance.hero.IncreaseStatPermanent(value, StatType.DMG);
                break;
            case ItemType.Ring:
                Debug.Log("using item ring");
                if(itemName == ItemName.AttackRing) GameManager.Instance.hero.IncreaseStatTemporary(value, StatType.DMG);
                if(itemName == ItemName.DodgeRing) GameManager.Instance.hero.IncreaseStatTemporary(value, StatType.Dodge);
                break;
            case ItemType.Buff:
                if(itemName == ItemName.Poison) GameManager.Instance.hero.effectList.AddEffectDurationValue(EffectName.Poison, 5, 5);
                if(itemName == ItemName.ContinuousHeal) GameManager.Instance.hero.effectList.AddEffect(EffectName.Heal);
                break;

        }
    }
}