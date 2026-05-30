/**
 * Script for door object, transfers between scenes
 */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    void OnMouseDown() {
        Debug.Log("Door clicked");

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
                    SceneManager.LoadScene("Room_Shop");
                    break;
                default:    //empty, treasure, fight 
                    GameManager.Instance.currentRoomType = type;
                    Debug.Log("Trying to enter " + type);
                    GameManager.Instance.IncreaseRoomCount();
                    CursorManager.Instance.RemoveRequest(this);
                    SceneManager.LoadScene("Room_Hallway");
                    break;
            }

            // if (type == RoomType.Back) {
                
            // } else if (type == RoomType.Wall) {
            //     Debug.Log("Trying to walk into a wall, dumbshit");
            // } else if (type == RoomType.Shop) {
                
            // } else {    //treasure, fight

            // }
            
        // leaving room
        } else if (!GameManager.Instance.combat) {
            GameManager.Instance.currentRoomType = RoomType.None;
            CursorManager.Instance.RemoveRequest(this);
            SceneManager.LoadScene("Room_Between");
        }
    }

    void OnMouseEnter() {
        CursorManager.Instance.AddRequest(this, CursorType.Grab, Prio.Background, "enter door");
    }

    void OnMouseExit() {
        CursorManager.Instance.RemoveRequest(this);
    }
}
