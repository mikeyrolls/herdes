/**
 * Main room setup
 */

using UnityEngine;
using System.Collections;

public class RoomGen : MonoBehaviour {

    [SerializeField] SpriteRenderer doorSprite;

    [SerializeField] Sprite doorBack;
    [SerializeField] Sprite doorBack2;

    [SerializeField] Sprite doorEmpty;
    [SerializeField] Sprite doorFight;
    [SerializeField] Sprite doorTreasure;
    [SerializeField] Sprite doorFakewall;
    [SerializeField] Sprite doorWall;
    [SerializeField] Sprite doorShop;

    private bool debug = true;


    void Start() {
        GameManager.Instance.currentDirection = Direction.Middle;
        if (debug) {
            GenerateRoomsDebug();
        } else {
            GenerateRooms();
        }
        resetDoorSprite();
    }

    void GenerateRooms() {
        ChangeRoomType(3, RoomType.Back);

        if  ((GameManager.Instance.roomCount + 1) % 5 == 0){
            ChangeRoomType(0, RoomType.Wall);
            ChangeRoomType(1, RoomType.Shop);
            ChangeRoomType(2, RoomType.Wall);

        } else {
            for (int i = 0; i < 3; i++) {

                RoomType newRoomType;

                int percent = Helper.GetPerc();
                if (percent <= 10) {    //10
                    newRoomType = RoomType.Treasure;
                } else if (percent <= 35) { //25
                    newRoomType = RoomType.Empty;
                } else if (percent <= 65) { //30
                    newRoomType = RoomType.Fight;
                } else if (percent <= 70) { //5
                    newRoomType = RoomType.Fakewall;
                } else {                    // 30
                    if(i == 1) { //middle
                        newRoomType = RoomType.Fight;
                    } else {
                        newRoomType = RoomType.Wall;
                    }
                }
                ChangeRoomType(i, newRoomType);
            }
        }
        Debug.Log("Roomtypes: left: " + GameManager.Instance.nextRooms[0].roomType + ", middle: " + GameManager.Instance.nextRooms[1].roomType + ", right: " + GameManager.Instance.nextRooms[2].roomType + ", back: " + GameManager.Instance.nextRooms[3].roomType);
    }

    void GenerateRoomsDebug() {
        
        ChangeRoomType(0, RoomType.Treasure);
        ChangeRoomType(1, RoomType.Shop);
        ChangeRoomType(2, RoomType.Fight);
        ChangeRoomType(3, RoomType.Fakewall);

        Debug.Log("Roomtypes: left: " + GameManager.Instance.nextRooms[0].roomType + ", middle: " + GameManager.Instance.nextRooms[1].roomType + ", right: " + GameManager.Instance.nextRooms[2].roomType + ", back: " + GameManager.Instance.nextRooms[3].roomType);
    }

    public void ChangeRoomType(int index, RoomType newRoomType) {
        Sprite newRoomSprite;

        switch(newRoomType) {
            case RoomType.Back:
                newRoomSprite = doorBack;
                break;
            case RoomType.Back2:
                newRoomSprite = doorBack2;
                break;
            case RoomType.Shop:
                newRoomSprite = doorShop;
                break;
            case RoomType.Treasure:
                newRoomSprite = doorTreasure;
                break;
            case RoomType.Empty:
                newRoomSprite = doorEmpty;
                break;
            case RoomType.Fight:
                newRoomSprite = doorFight;
                break;
            case RoomType.Fakewall:
                newRoomSprite = doorFakewall;
                break;
            case RoomType.Wall:
            default:
                newRoomSprite = doorWall;
                break;
        }
        GameManager.Instance.nextRooms[index].ChangeRoomType(newRoomType, newRoomSprite);

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

    public void resetDoorSprite() {
        //if (GameManager.Instance.currentDirection == Direction.Left) {
            int roomIndex = (int)GameManager.Instance.currentDirection;
            doorSprite.sprite = GameManager.Instance.nextRooms[roomIndex].roomSprite;
            Debug.Log("facing " + GameManager.Instance.currentDirection);

        // } else if (GameManager.Instance.currentDirection == Direction.Middle) {
        //     doorSprite.sprite = doorMiddle;
        // } else if (GameManager.Instance.currentDirection == Direction.Right) {
        //     doorSprite.sprite = doorRight;
        // } else if (GameManager.Instance.currentDirection == Direction.Back) {
        //     doorSprite.sprite = doorBack;
        // }
    }

}
