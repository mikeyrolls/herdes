/**
 * Item game object class, accesses data
 */

using UnityEngine;

public class ItemGO : MonoBehaviour {
    public Item data = new Item();
    public ItemSpriteDB itemSpriteDB;
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
        UIManager.Instance.CursorSetHoverGrab();
    }

    void OnMouseExit() {
        UIManager.Instance.CursorSetDefault();
    }

    public void InitializeFromDB(ItemName itemName) {
        data.InitializeFromDB(itemName, itemSpriteDB);
        sr.sprite = data.itemSprite;
    }

    public void InitGold(int value) {
        data.InitGold(value, itemSpriteDB);
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