/**
 * Item game object class, accesses data
 */

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ItemGO : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public Item data = new Item();
    public ItemSpriteDB itemSpriteDB;
    private SpriteRenderer sr;

    Inventory inventory;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        inventory = GameManager.Instance.hero.inventory;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        CursorManager.Instance.AddRequest(this, CursorType.Grab, Prio.World, "take");
    }

    public void OnPointerExit(PointerEventData eventData) {
        CursorManager.Instance.RemoveRequest(this);
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnClick();
    }

    void OnClick() {
        if (inventory.addToInventory(data)) {
            Destroy(gameObject);
            CursorManager.Instance.RemoveRequest(this);
        }
    }

    //--------------------------------------

    public void InitializeFromData(Item item) { //unused for now
        this.data = item;
        sr.sprite = data.itemSprite;
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