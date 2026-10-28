/**
 * Hover script for buttons
 */

using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public CursorType cursorType;
    public string textOnHover;
    public bool reactiWhileHolding;
    public string textOnHoverHolding;

    public void OnPointerEnter(PointerEventData e) {
        CursorManager.Instance.AddRequest(this, cursorType, Prio.UI, textOnHover);
        if (reactiWhileHolding) CursorManager.Instance.AddRequest(this, cursorType, Prio.UI, textOnHoverHolding, true);
    }

    public void OnPointerExit(PointerEventData e) {
        CursorManager.Instance.RemoveRequest(this);
    }

    void OnDisable() {
       if (CursorManager.Instance != null)
           CursorManager.Instance.RemoveRequest(this);
    }
}