/**
 * Script for door object, transfers between scenes
 */

using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    void OnMouseDown() {
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
                    FindAnyObjectByType<RoomGen>()?.ChangeRoomType(dir, RoomType.Back2);
                    FindAnyObjectByType<RoomGen>()?.resetDoorSprite();
                    break;
                case RoomType.Back2:
                case RoomType.Wall:
                    Debug.Log("Trying to walk into a wall, dumbshit");
                    break;
                case RoomType.Shop:
                    GameManager.Instance.currentRoomType = type;
                    Debug.Log("Trying to enter " + type);
                    GameManager.Instance.IncreaseRoomCount();
                    CursorManager.Instance.RemoveRequest(this);
                    StartCoroutine(GameManager.Instance.LoadNewScene("Room_Shop"));
                    break;
                default:    //empty, treasure, fight 
                    GameManager.Instance.currentRoomType = type;
                    Debug.Log("Trying to enter " + type);
                    GameManager.Instance.IncreaseRoomCount();
                    CursorManager.Instance.RemoveRequest(this);
                    StartCoroutine(GameManager.Instance.LoadNewScene("Room_Hallway"));
                    break;
            }

            
        // leaving room
        } else if (!GameManager.Instance.combat) {
            GameManager.Instance.currentRoomType = RoomType.None;
            CursorManager.Instance.RemoveRequest(this);
            StartCoroutine(GameManager.Instance.LoadNewScene("Room_Between"));
        }
    }

    void OnMouseEnter() {
        CursorManager.Instance.AddRequest(this, CursorType.Grab, Prio.Background, "enter door");
    }

    void OnMouseExit() {
        CursorManager.Instance.RemoveRequest(this);
    }
}
