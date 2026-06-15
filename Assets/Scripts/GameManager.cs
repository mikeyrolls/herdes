/**
 * Main logic handles, hell on earth of code
 */

using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    //surviving objects
    public static GameManager Instance;
    //GameObject heroGO; ueh
    public Hero hero;
    
    // none room navig 
    public int reservedRooms = 3;
    public RoomType currentRoomType = RoomType.None;
    [NonSerialized] public Room[] nextRooms;
    public Direction currentDirection = Direction.Middle;

    // gen
    public int roomCount = 0;
    public bool combat = false;

    void Start() {
        Debug.Log("in gameman start");
        UIManager.Instance.RefreshHUD();
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitNewRun();
        } else {
            Destroy(gameObject);
        }
    }

    public void IncreaseRoomCount() {
        roomCount++;
        UIManager.Instance.RefreshHUD();
    }

    public void InitNewRun() {
        hero = new Hero();
        hero.InitializeFromDB(HeroType.Fishbone);

        roomCount = 0;
        currentRoomType = RoomType.None;
        currentDirection = Direction.Middle;
        combat = false;

        nextRooms = new Room[4];
        for (int i = 0; i < 4; i++)
            nextRooms[i] = new Room();
        
        UIManager.Instance.Init();
    }

    public void Die() {
        UIManager.Instance.ShowGameOver();
    }

    public IEnumerator LoadNewScene(string sceneName) {
        yield return new WaitUntil(() => !Input.GetMouseButton(0));
        SceneManager.LoadScene(sceneName);
    }

    public void Retry() {
        InitNewRun();
        UIManager.Instance.HideGameOver();
        StartCoroutine(LoadNewScene("Room_Between"));
    }

}
