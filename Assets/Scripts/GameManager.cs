/**
 * Main logic handles, hell on earth of code
 */

using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    //surviving objects
    public static GameManager Instance;
    GameObject heroGO;
    public Hero hero;
    
    // none room navig 
    public int reservedRooms = 3;
    public Room currentRoom = Room.None;
    [NonSerialized] public Room[] nextRooms = new Room[4];
    public Direction currentDirection = Direction.Middle;

    // gen
    public int roomCount = 0;
    public bool combat = false;

    void Start() {
        Debug.Log("in gameman start");
        UIManager.Instance.RefreshHUD();
        nextRooms[3] = Room.Back;
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            heroGO = new GameObject("Hero");
            heroGO.transform.SetParent(transform); // hero survives as child of GameManager
            hero = heroGO.AddComponent<Hero>();
            hero.InitializeFromDB(HeroType.Fishbone);
        } else {
            Destroy(gameObject);
        }
    }

    public void IncreaseRoomCount() {
        roomCount++;
        UIManager.Instance.RefreshHUD();
    }

    public void addToInventory(Item item) {
        if (item.itemType == ItemType.Gold) {
            hero.updateMoney(item.value);
        } else {
            //todo
        }
        UIManager.Instance.RefreshHUD();
    }

}
