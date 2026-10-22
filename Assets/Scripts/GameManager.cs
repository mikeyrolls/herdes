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

    // for scene switches
    public bool isTransitioning;

    public HeroSpriteSet FishboneSprites;
    public HeroSpriteSet DefaultSprites;

    void Start() { //after awake
        InitNewRun();
        UIManager.Instance.RefreshHUD();
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
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
        hero.spriteSet = FishboneSprites;
        CursorManager.Instance.SwitchCursors((HeroSpriteSet)hero.spriteSet);
        Debug.Log("hero sprites = " + hero.spriteSet);

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
        StartCoroutine(UIManager.Instance.ShowGameOver());
    }

    public void LoadScene(string sceneName) {
        if (isTransitioning) return;
        isTransitioning = true;
        StartCoroutine(LoadNewScene(sceneName));
    }

    IEnumerator LoadNewScene(string sceneName) {
        // if fade, here
        var op = SceneManager.LoadSceneAsync(sceneName);
        yield return op;
        isTransitioning = false;
    }

    public void Retry() {
        InitNewRun();
        UIManager.Instance.HideGameOver();
        StartCoroutine(LoadNewScene("Room_Between"));
    }

    public void SetDefaultSprites() {
        CursorManager.Instance.SwitchCursors(DefaultSprites);
    }

}
