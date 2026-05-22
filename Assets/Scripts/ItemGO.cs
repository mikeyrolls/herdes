/**
 * Item game object class, accesses data
 */

using UnityEngine;

public class ItemGO : MonoBehaviour {
    public Item data = new Item();
    public ItemSpriteDatabase itemSpriteSet;
    private SpriteRenderer sr;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {
        if(GameManager.Instance.addToInventory(data)) {
            Destroy(gameObject);
            UIManager.Instance.CursorSetDefault();
        }
    }

    void OnMouseEnter() {
        UIManager.Instance.CursorSetHoverEnemy();
    }

    void OnMouseExit() {
        UIManager.Instance.CursorSetDefault();
    }

    public void InitializeFromDB(ItemName itemName) {
        data.InitializeFromDB(itemName, itemSpriteSet);
        sr.sprite = data.itemSprite;
    }

    public void InitGold(int value) {
        data.InitGold(value, itemSpriteSet);
        sr.sprite = data.itemSprite;
    }

    public Sprite GetSprite() {
        return sr.sprite;
    }

    public string GetName() {
        return data.nameStr;
    }

    public ItemType GetItemType() {
        return data.itemType;
    }

    public int GetValue() {
        return data.value;
    }
}