/**
 * Script for door object, transfers between scenes
 */

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    bool hovered;
    bool cursorActive;
    HallwayManager hallway;

    void Update() {
        if (hovered) 
            RefreshCursor();
    }

    void RefreshCursor() {
        if (hallway == null) hallway = FindAnyObjectByType<HallwayManager>();

        bool isInCombat = GameManager.Instance.combat;
        bool blocked = isInCombat && !hallway.IsPlayerTurn;
        bool wantGrab = hovered && !blocked;

        if (wantGrab && !cursorActive) {
            if (isInCombat) CursorManager.Instance.AddRequest(this, CursorType.Leave, Prio.Background, "try to flee");
            else CursorManager.Instance.AddRequest(this, CursorType.Leave, Prio.Background, "enter door");
            cursorActive = true;
        } else if (!wantGrab && cursorActive) {
            CursorManager.Instance.RemoveRequest(this);
            cursorActive = false;
        }
    }

    public void OnPointerClick(PointerEventData e) {
        if (GameManager.Instance.isTransitioning) return;

        // entering room
        if (GameManager.Instance.currentRoomType == RoomType.None) {

            int dir = (int)GameManager.Instance.currentDirection;
            RoomType type = GameManager.Instance.nextRooms[dir].roomType;
            

            switch (type) {
                case RoomType.Back:
                    FindAnyObjectByType<HubRoomManager>()?.ChangeRoomType(dir, RoomType.Back2);
                    FindAnyObjectByType<HubRoomManager>()?.ResetDoorSprite();
                    break;
                case RoomType.Back2:
                case RoomType.Wall:
                    break;
                case RoomType.Shop:
                    GameManager.Instance.currentRoomType = type;
                    GameManager.Instance.IncreaseRoomCount();
                    GameManager.Instance.LoadScene("Room_Shop");
                    break;
                default:    //empty, treasure, fight 
                    GameManager.Instance.currentRoomType = type;
                    GameManager.Instance.IncreaseRoomCount();
                    GameManager.Instance.LoadScene("Room_Hallway");
                    break;
            }

            
        // leaving room
        } else if (!GameManager.Instance.combat) {
            GameManager.Instance.currentRoomType = RoomType.None;
            GameManager.Instance.LoadScene("Room_Between");
        } else {
            //fleeing from combat
            StartCoroutine(FindAnyObjectByType<HallwayManager>().OnDoorClicked());
        }
    }

    public void OnPointerEnter(PointerEventData e) {
        hovered = true;
        RefreshCursor();
    }

    public void OnPointerExit(PointerEventData e) {
        hovered = false;
        RefreshCursor();
    }

    public void OnDisable() {
        hovered = false;
        cursorActive = false;
        if (CursorManager.Instance != null) {
            CursorManager.Instance.RemoveRequest(this);
        }
    }

}
