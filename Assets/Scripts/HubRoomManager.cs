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

    [SerializeField] SpriteRenderer background;
    [SerializeField] Sprite bg1;
    [SerializeField] Sprite bg2;
    [SerializeField] Sprite bg3;

    private bool debug = true;

    [SerializeField] GameObject heroPrefab;
    HeroRoomGO heroGO;
    Hero hero;

    bool isRotating;

    void Start() {
        hero = GameManager.Instance.hero;

        heroGO = Instantiate(heroPrefab, new Vector3(-5.5f, -2f, 0f), Quaternion.identity).GetComponent<HeroRoomGO>();
        heroGO.InitGO(hero.spriteSet);
        
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
        if (isRotating) return;
        StartCoroutine(Rotate(Direction.Left));
        GameManager.Instance.currentDirection = (Direction)(((int)GameManager.Instance.currentDirection - 1 + 4) % 4);
    }

    public void RotateRight() {
        if (isRotating) return;
        StartCoroutine(Rotate(Direction.Right));
        GameManager.Instance.currentDirection = (Direction)(((int)GameManager.Instance.currentDirection + 1) % 4);
    }

    IEnumerator Rotate(Direction direction) {
        isRotating = true;
        bool isLeft = direction == Direction.Left;

        float duration = 0.7f;
        // block input
        UIManager.Instance.BlockScreenFor(duration);

        // (hide kitty), dim door
        float elapsed = 0f;
        while (elapsed < duration / 4) {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / duration * 4);
            doorSprite.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        doorSprite.color = new Color(1f, 1f, 1f, 0f);

        // rotate 2 3 3rev 2rev 1 (5 in total)
        float t = duration / 2 / 5;
        background.sprite = bg2; 
        background.flipX = isLeft;
        yield return new WaitForSeconds(t);
        background.sprite = bg3;
        yield return new WaitForSeconds(t);
        background.flipX = !isLeft;
        yield return new WaitForSeconds(t);
        background.sprite = bg2;
        yield return new WaitForSeconds(t);
        background.sprite = bg1; 
        background.flipX = false;

        // reset door
        ResetDoorSprite();

        // undim door, (unhide kitty)
        elapsed = 0f;
        while (elapsed < duration / 4) {
            elapsed += Time.deltaTime;
            float alpha = (elapsed / duration * 4);
            doorSprite.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        doorSprite.color = new Color(1f, 1f, 1f, 1f);

        //end
        isRotating = false;
    }

    public void ResetDoorSprite() {
            int roomIndex = (int)GameManager.Instance.currentDirection;
            doorSprite.sprite = GameManager.Instance.nextRooms[roomIndex].roomSprite;
    }

}
