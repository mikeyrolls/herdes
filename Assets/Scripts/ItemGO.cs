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
            CursorManager.Instance.RemoveRequest(this);
        }
    }

    void OnMouseEnter() {
        CursorManager.Instance.AddRequest(this, CursorType.Grab, Prio.World, "take");
    }

    void OnMouseExit() {
        CursorManager.Instance.RemoveRequest(this);
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