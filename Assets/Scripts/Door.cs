/**
 * Script for door object, transfers between scenes
 */

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Door : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public void OnPointerClick(PointerEventData e) {
        if (GameManager.Instance.isTransitioning) return;
        Debug.Log("Door clicked");

        if(!GameManager.Instance.hero.IsAlive()) {
            GameManager.Instance.Die();
            return;
        }

        // entering room
        if (GameManager.Instance.currentRoomType == RoomType.None) {

            int dir = (int)GameManager.Instance.currentDirection;
            RoomType type = GameManager.Instance.nextRooms[dir].roomType;
            

            switch (type) {
                case RoomType.Back:
                    Debug.Log("Trying to walk back, kill them");
                    FindAnyObjectByType<HubRoomManager>()?.ChangeRoomType(dir, RoomType.Back2);
                    FindAnyObjectByType<HubRoomManager>()?.ResetDoorSprite();
                    break;
                case RoomType.Back2:
                case RoomType.Wall:
                    Debug.Log("Trying to walk into a wall, dumbshit");
                    break;
                case RoomType.Shop:
                    GameManager.Instance.currentRoomType = type;
                    Debug.Log("Trying to enter " + type);
                    GameManager.Instance.IncreaseRoomCount();
                    GameManager.Instance.LoadScene("Room_Shop");
                    break;
                default:    //empty, treasure, fight 
                    GameManager.Instance.currentRoomType = type;
                    Debug.Log("Trying to enter " + type);
                    GameManager.Instance.IncreaseRoomCount();
                    GameManager.Instance.LoadScene("Room_Hallway");
                    break;
            }

            
        // leaving room
        } else if (!GameManager.Instance.combat) {
            GameManager.Instance.currentRoomType = RoomType.None;
            CursorManager.Instance.RemoveRequest(this);
            GameManager.Instance.LoadScene("Room_Between");
        }
    }

    public void OnPointerEnter(PointerEventData e) {
        CursorManager.Instance.AddRequest(this, CursorType.Grab, Prio.Background, "enter door");
    }

    public void OnPointerExit(PointerEventData e) {
        CursorManager.Instance.RemoveRequest(this);
    }

    public void OnDisable() {
        if (CursorManager.Instance != null) {
            CursorManager.Instance.RemoveRequest(this);
        }
    }

}
