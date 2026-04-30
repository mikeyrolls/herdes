/**
 * UI handler
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
        FindAnyObjectByType<RoomGen>()?.RotateLeft();
        Debug.Log("button wants left");
    }

    public void OnRotateRight() {
        FindAnyObjectByType<RoomGen>()?.RotateRight();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        bool isNoneRoom = GameManager.Instance.currentRoom == Room.None;
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        if (isNoneRoom) StartCoroutine(EnableButtonsNextFrame());
    }

    private IEnumerator EnableButtonsNextFrame() {
        yield return null;
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
    }

    public void RefreshHUD() {
        moneyText.text = GameManager.Instance.hero.gold.ToString() + " gold";
        roomCountText.text = "Room num: " + GameManager.Instance.roomCount.ToString();
        hpText.text = "HP: " + GameManager.Instance.hero.currHP.ToString() + "/" + GameManager.Instance.hero.maxHP.ToString();

        if(GameManager.Instance.hero.GetHpPerc() > 50) {
            stateIcon.sprite = stateIconHp1;
        } else {
            stateIcon.sprite = stateIconHp2;
        }
    }

    public void CursorSetDefault() => Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    public void CursorSetHoverEnemy() => Cursor.SetCursor(hoverEnemyCursor, Vector2.zero, CursorMode.Auto);

}
