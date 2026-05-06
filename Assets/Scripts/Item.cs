/**
 * Item game object and script object
 */

using UnityEngine;

public class Item : MonoBehaviour {

    public string nameStr;
    public int price;
    public int value;
    public ItemType itemType;

    public Sprite itemSprite;
    public ItemSpriteDatabase itemSpriteSet;
    private SpriteRenderer sr;

    public void InitializeFromDB(ItemName itemName) {

        Debug.Log("attempting init from db spawn");

        if (ItemDB.items.TryGetValue(itemName, out var itemData)) {
            nameStr = itemData.displayName;
            value = itemData.value;
            price = itemData.price;
            itemType = itemData.itemType;
            sr.sprite = itemSpriteSet.GetSprite(itemName);
            
            Debug.Log($"Spawned {nameStr}");
        } else {
            Debug.LogError($"Item '{itemName.ToString()}' not found in database!");
        }
    }

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {
        GameManager.Instance.addToInventory(this);
        Destroy(gameObject);
    }
}

