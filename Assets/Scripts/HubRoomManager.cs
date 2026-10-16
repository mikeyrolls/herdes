/**
 * Room generation and hub manager
 */

using UnityEngine;
using System.Collections;

public class HubRoomManager : MonoBehaviour {

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

    [SerializeField] GameObject heroPrefab;
    HeroRoomGO heroGO;
    Hero hero;
    public RoomSpriteSet roomSpriteSet;

    void Start() {
        hero = GameManager.Instance.hero;

        heroGO = Instantiate(heroPrefab, new Vector3(-5.5f, -3f, 0f), Quaternion.identity).GetComponent<HeroRoomGO>();
        heroGO.InitGO(roomSpriteSet);
        hero.sceneObject = heroGO;

        heroGO.onDeath = () => GameManager.Instance.Die();


        GameManager.Instance.currentDirection = Direction.Middle;
        GameManager.Instance.hero.ApplyEffects();
        if (debug) {
            GenerateRoomsDebug();
        } else {
            GenerateRooms();
        }
        ResetDoorSprite();
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
    }

    void GenerateRoomsDebug() {
        ChangeRoomType(0, RoomType.Treasure);
        ChangeRoomType(1, RoomType.Shop);
        ChangeRoomType(2, RoomType.Fight);
        ChangeRoomType(3, RoomType.Fakewall);
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
        ResetDoorSprite();
    }

    public void RotateRight() {
        GameManager.Instance.currentDirection = (Direction)(((int)GameManager.Instance.currentDirection + 1) % 4);
        ResetDoorSprite();
    }

    public void ResetDoorSprite() {
            int roomIndex = (int)GameManager.Instance.currentDirection;
            doorSprite.sprite = GameManager.Instance.nextRooms[roomIndex].roomSprite;
    }

}
