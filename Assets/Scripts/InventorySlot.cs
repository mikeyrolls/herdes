/**
 * Single inventory slot UI element
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    [SerializeField] Image itemImage;
    Item item;
    public int slotIndex;

    Inventory inventory;

    float holdThreshold = 0.2f;
    bool holdFired = false;
    Coroutine holdCoroutine;
    bool isHolding = false;

    public void Init(Inventory inventory, int index) {
        slotIndex = index;
        this.inventory = inventory;
    }

    bool isEmpty() {
        return item == null;
    }

   public void OnPointerEnter(PointerEventData eventData) {
        if (!isEmpty()) {
            CursorManager.Instance.AddRequest(this, CursorType.Grab, Prio.UI, "use");
        } else {
            CursorManager.Instance.AddRequest(this, CursorType.Normal, Prio.UI, "empty");
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        CursorManager.Instance.RemoveRequest(this);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (isEmpty()) return;
        if (holdFired) return;
        if (eventData.clickCount == 2)
            OnDoubleClick();
        else if (eventData.clickCount == 1)
            OnClick();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (isEmpty()) return;
        holdFired = false;
        holdCoroutine = StartCoroutine(HoldTimer());
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (holdCoroutine != null)
            StopCoroutine(holdCoroutine);

        if (isHolding) {
            isHolding = false;
            OnHoldRelease(eventData);
        }
    }

    IEnumerator HoldTimer() {
        Debug.Log("Hold timer started");
        yield return new WaitForSeconds(holdThreshold);
        holdFired = true;
        isHolding = true;
        OnHold();
    }

    void OnClick() {
        Debug.Log("Clicked once");
    }

    void OnDoubleClick() {
        if(slotIndex < 6) inventory.UseFromInventory(slotIndex);
        else inventory.UnequipFromInventory(slotIndex);
        //UpdateSprite();
    }

    void OnHold() {
        Debug.Log("Holding item?");
    }

    void OnHoldRelease(PointerEventData eventData) {

        Debug.Log("Released hold");

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results) {
            InventorySlot target = result.gameObject.GetComponent<InventorySlot>();
            if (target != null && target != this) {
                inventory.Swap(slotIndex, target.slotIndex);
                return;
            }
        }
    }

    public void UpdateSprite() {
        item = inventory.itemInventory[slotIndex];
        if (item == null) {
            itemImage.gameObject.SetActive(false);
        } else {
            itemImage.sprite = item.itemSprite;
            itemImage.gameObject.SetActive(true);
        }
    }
}
