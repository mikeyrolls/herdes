using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    [SerializeField] TMP_Text roomCountText;
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text hpText;

    [SerializeField] Image stateIcon;
    [SerializeField] Sprite stateIconHp1;
    [SerializeField] Sprite stateIconHp2;

    [SerializeField] Texture2D defaultCursor;
    [SerializeField] Texture2D hoverEnemyCursor;


    void Start() {
        SceneManager.LoadScene("Room_Between");
        RefreshHUD();

        CursorSetDefault();
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void OnRotateLeft() {
        FindObjectOfType<RoomGen>()?.RotateLeft();
        Debug.Log("button wants left");
    }

    public void OnRotateRight() {
        FindObjectOfType<RoomGen>()?.RotateRight();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        bool isNoneRoom = GameManager.Instance.currentRoom == GameManager.Room.none;
        leftButton.gameObject.SetActive(isNoneRoom);
        rightButton.gameObject.SetActive(isNoneRoom);
    }

    public void RefreshHUD() {
        moneyText.text = GameManager.Instance.moneyAmount.ToString() + " gold";
        roomCountText.text = "Room num: " + GameManager.Instance.roomCount.ToString();
        hpText.text = "HP: " + GameManager.Instance.currHp.ToString() + "/" + GameManager.Instance.maxHp.ToString();

        if(GameManager.Instance.getHpPerc() > 50) {
            stateIcon.sprite = stateIconHp1;
        } else {
            stateIcon.sprite = stateIconHp2;
        }
    }

    public void CursorSetDefault() => Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    public void CursorSetHoverEnemy() => Cursor.SetCursor(hoverEnemyCursor, Vector2.zero, CursorMode.Auto);

}
