using UnityEngine;
using System;
using Random = UnityEngine.Random;



public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public enum Room{none, back, shop, wall, fight, treasure } //fakewall, fishing, ...
    public int reservedRooms = 3;
    public int roomAmount = Enum.GetNames(typeof(Room)).Length;
    public Room currentRoom = Room.none;
    [NonSerialized] public Room[] nextRooms = new Room[4];
    
    public enum Direction{left, middle, right, back}
    public Direction currentDirection = Direction.middle;

    public int roomCount = 0;
    public bool combat = false;
    public int moneyAmount = 0;

    public int maxHp;
    public int currHp;
    int dmgLow;
    int dmgHigh;
    string name;

    void Start() {
        Debug.Log("nextrooms length: " + nextRooms.Length);
        nextRooms[3] = Room.back;
        maxHp = 10;
        currHp = maxHp;
        dmgHigh = 5;
        dmgLow = 5;
        name = "Fishbone";
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // survives scene loads
        } else {
            Destroy(gameObject); // prevents duplicates
        }
    }

    public void IncreaseRoomCount() {
        roomCount++;
        UIManager.Instance.RefreshHUD();
        
    }

    public void addToInventory(Loot loot) {
        if(loot.lootType == Loot.LootType.gold) {
            moneyAmount += loot.value;
        }
        UIManager.Instance.RefreshHUD();
    }

    public void takeDmg(int rawDmg) {
        currHp -= rawDmg;
        if (currHp < 0) {
            currHp = 0;
        }
        UIManager.Instance.RefreshHUD();
    }

    public int giveDmg() {
        int rawDmg = Random.Range(dmgLow, dmgHigh);
        return rawDmg;
    }

    public int getHpPerc() {
        return 100 * currHp / maxHp;
    }

}
