/**
 * Main room setup
 */

using UnityEngine;
using System.Collections;

public class RoomGen : MonoBehaviour {

    [SerializeField] SpriteRenderer doorSprite;
    [SerializeField] Sprite doorLeft;
    [SerializeField] Sprite doorMiddle;
    [SerializeField] Sprite doorRight;
    [SerializeField] Sprite doorBack;


    void Start() {
        GameManager.Instance.currentDirection = Direction.Middle;
        GenerateRooms();
    }

    void GenerateRooms() {
        if(GameManager.Instance.roomCount + 1 % 5 == 0) {

        } else {
            for (int i = 0; i < 3; i++) {
                Room pickedRoom = (Room)Random.Range(
                    GameManager.Instance.reservedRooms + (i % 2), // start after reserved, middle can't be wall ever
                    Helper.roomAmount
                );
                GameManager.Instance.nextRooms[i] = pickedRoom;
                
            }
        }
        Debug.Log("Roomtypes: left: " + GameManager.Instance.nextRooms[0] + ", middle: " + GameManager.Instance.nextRooms[1] + ", right: " + GameManager.Instance.nextRooms[2] + ", back: " + GameManager.Instance.nextRooms[3]);
    }

    public void RotateLeft() {
        GameManager.Instance.currentDirection = (Direction)(((int)GameManager.Instance.currentDirection - 1 + 4) % 4);
        Debug.Log("left room func called");
        resetDoorSprite();
    }

    public void RotateRight() {
        GameManager.Instance.currentDirection = (Direction)(((int)GameManager.Instance.currentDirection + 1) % 4);
        resetDoorSprite();
    }

    void resetDoorSprite() {
        if (GameManager.Instance.currentDirection == Direction.Left) {
            doorSprite.sprite = doorLeft;
            Debug.Log("after left sprite");
        } else if (GameManager.Instance.currentDirection == Direction.Middle) {
            doorSprite.sprite = doorMiddle;
        } else if (GameManager.Instance.currentDirection == Direction.Right) {
            doorSprite.sprite = doorRight;
        } else if (GameManager.Instance.currentDirection == Direction.Back) {
            doorSprite.sprite = doorBack;
        }
    }

}
