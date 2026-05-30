/**
 * Single inventory slot UI element
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    [SerializeField] Image itemImage;
    Item item;
    public int slotIndex;


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
        GameManager.Instance.UseFromInventory(slotIndex);
    }

    public void UpdateSprite() {
        item = GameManager.Instance.hero.inventory[slotIndex];
        if (item == null) {
            itemImage.gameObject.SetActive(false);
        } else {
            Debug.Log("inv space "+ slotIndex +" has " + item.nameStr);
            itemImage.sprite = item.itemSprite;
            itemImage.gameObject.SetActive(true);
        }
    }
}
