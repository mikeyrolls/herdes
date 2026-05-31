/**
 * Item game object class, accesses data
 */

using UnityEngine;
using System.Collections;

public class ItemGO : MonoBehaviour {
    public Item data = new Item();
    public ItemSpriteDB itemSpriteDB;
    private SpriteRenderer sr;

    float lastClickTime;
    float doubleClickThreshold = 0.3f;
    float holdThreshold = 0.5f;

    bool holdFired = false;
    Coroutine holdCoroutine;

    Inventory inventory = GameManager.Instance.hero.inventory;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown() {

        if (Time.time - lastClickTime < doubleClickThreshold) {
            OnDoubleClick();
            return;
        }
        lastClickTime = Time.time;

        holdFired = false;
        holdCoroutine = StartCoroutine(HoldTimer());
    }

    void OnMouseUp() {
        if (holdCoroutine != null)
            StopCoroutine(holdCoroutine);

        if (!holdFired)
            OnClick();
    }

    // clicking on item adds it to inventory if possible
    void OnClick() {
        if(inventory.addToInventory(data)) {
            Destroy(gameObject);
            CursorManager.Instance.RemoveRequest(this);
        }
    }

    void OnHold() {

    }

    // double click uses the item immediately if possible
    void OnDoubleClick() {  

    }

    IEnumerator HoldTimer() {
        yield return new WaitForSeconds(holdThreshold);
        holdFired = true;
        OnHold();
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