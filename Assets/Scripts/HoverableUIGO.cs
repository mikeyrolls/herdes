/**
 * Hover script for buttons
 */

using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public CursorType cursorType;
    public string textOnHover;

    public void OnPointerEnter(PointerEventData e) {
        CursorManager.Instance.AddRequest(this, cursorType, Prio.UI, textOnHover);
    }

    public void OnPointerExit(PointerEventData e) {
        CursorManager.Instance.RemoveRequest(this);
    }
}