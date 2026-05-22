/**
 * Main logic handles, hell on earth of code
 */

using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    //surviving objects
    public static GameManager Instance;
    //GameObject heroGO; ueh
    public Hero hero;
    
    // none room navig 
    public int reservedRooms = 3;
    public RoomType currentRoomType = RoomType.None;
    [NonSerialized] public Room[] nextRooms = new Room[4];
    public Direction currentDirection = Direction.Middle;

    // gen
    public int roomCount = 0;
    public bool combat = false;

    void Start() {
        Debug.Log("in gameman start");
        UIManager.Instance.RefreshHUD();
        
        for (int i = 0; i < 4; i++) {
            nextRooms[i] = new Room();
        }

    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // heroGO = new GameObject("Hero");
            // heroGO.transform.SetParent(transform); // hero survives as child of GameManager
            // hero = heroGO.AddComponent<Hero>();

            hero = new Hero();
            hero.InitializeFromDB(HeroType.Fishbone);
        } else {
            Destroy(gameObject);
        }
    }

    public void IncreaseRoomCount() {
        roomCount++;
        UIManager.Instance.RefreshHUD();
    }

    public bool addToInventory(Item item) {
        if (item.itemType == ItemType.Gold) {
            hero.updateMoney(item.value);
        } else {
            int firstEmpty = hero.GetEmptyInvSlot();
            if (firstEmpty < hero.GetInvSize()) {
                hero.inventory[firstEmpty] = item;
                Debug.Log("added " + item.nameStr + " on inv space " + firstEmpty); 
            } else {
                return false;
            }
        }
        UIManager.Instance.RefreshHUD();
        return true;
    }

    public void UseFromInventory(int index) {
        if (index >= hero.GetInvSize()) {
            Debug.LogError("Out of bounds");
            return;
        }
        if (hero.inventory[index] == null) return;

        hero.inventory[index].UseItem();
        hero.inventory[index] = null;
        UIManager.Instance.RefreshHUD();
    }

}
