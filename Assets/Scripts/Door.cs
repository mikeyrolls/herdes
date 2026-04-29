using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    void OnMouseDown() {
        Debug.Log("Door clicked");

        // entering room
        if (GameManager.Instance.currentRoom == GameManager.Room.none) {

            GameManager.Room type = GameManager.Instance.nextRooms[(int)GameManager.Instance.currentDirection];
            

            if (type == GameManager.Room.back) {
                Debug.Log("Trying to walk back, kill them");
            } else if (type == GameManager.Room.wall) {
                Debug.Log("Trying to walk into a wall, dumbshit");
            } else if (type == GameManager.Room.shop) {
                Debug.Log("shop doesn't exist yet");
                GameManager.Instance.IncreaseRoomCount();
            } else {    //treasure, fight
                GameManager.Instance.currentRoom = type;
                Debug.Log("Trying to enter " + type);
                GameManager.Instance.IncreaseRoomCount();
                SceneManager.LoadScene("Room_Hallway");
            }
            
            

        // leaving room
        } else if (!GameManager.Instance.combat) {
            GameManager.Instance.currentRoom = GameManager.Room.none;
            SceneManager.LoadScene("Room_Between");
        }

    }



}
