using UnityEngine;
using System.Collections;

public class RoomGen : MonoBehaviour {

    [SerializeField] SpriteRenderer doorSprite;
    [SerializeField] Sprite doorLeft;
    [SerializeField] Sprite doorMiddle;
    [SerializeField] Sprite doorRight;
    [SerializeField] Sprite doorBack;


    void Start() {
        GameManager.Instance.currentDirection = GameManager.Direction.middle;
        GenerateRooms();

    }

    void GenerateRooms() {
        if(GameManager.Instance.roomCount + 1 % 5 == 0) {

        } else {
            for (int i = 0; i < 3; i++) {
                GameManager.Room pickedRoom = (GameManager.Room)Random.Range(
                    GameManager.Instance.reservedRooms + (i % 2), // start after reserved, middle can't be wall ever
                    GameManager.Instance.roomAmount
                );
                GameManager.Instance.nextRooms[i] = pickedRoom;
                Debug.Log("picked roomType " + pickedRoom + " for room " + i);
            }
        }
    }

    public void RotateLeft() {
        GameManager.Instance.currentDirection = (GameManager.Direction)(((int)GameManager.Instance.currentDirection - 1 + 4) % 4);
        Debug.Log("left room func called");
        resetDoorSprite();
    }

    public void RotateRight() {
        GameManager.Instance.currentDirection = (GameManager.Direction)(((int)GameManager.Instance.currentDirection + 1) % 4);
        resetDoorSprite();
    }

    void resetDoorSprite() {
        if (GameManager.Instance.currentDirection == GameManager.Direction.left) {
            doorSprite.sprite = doorLeft;
            Debug.Log("after left sprite");
        } else if (GameManager.Instance.currentDirection == GameManager.Direction.middle) {
            doorSprite.sprite = doorMiddle;
        } else if (GameManager.Instance.currentDirection == GameManager.Direction.right) {
            doorSprite.sprite = doorRight;
        } else if (GameManager.Instance.currentDirection == GameManager.Direction.back) {
            doorSprite.sprite = doorBack;
        }
    }

}
