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
    [SerializeField] Sprite stateIconHp3;
    [SerializeField] Sprite stateIconHp4;
    [SerializeField] Slider hpSlider;

    [SerializeField] Texture2D defaultCursor;
    [SerializeField] Texture2D hoverEnemyCursor;
    [SerializeField] Texture2D hoverGrabCursor;

    [SerializeField] InventorySlot[] inventorySlots = new InventorySlot[10];

    [SerializeField] private ActiveEffectsUI activeEffectsUI;


    [SerializeField] GameObject infobox;
    [SerializeField] TMP_Text infoBoxName;
    [SerializeField] TMP_Text infoBoxDesc;
    [SerializeField] TMP_Text infoBoxType;

    [SerializeField] GameObject gameOverScreen;


    void Start() {
        StartCoroutine(GameManager.Instance.LoadNewScene("Room_Between"));
        Init();
    }

    public void Init() {
        hpSlider.minValue = 0f;
        hpSlider.maxValue = 100f;

        for(int i = 0; i < 10; i++) {
            inventorySlots[i].Init(GameManager.Instance.hero.inventory, i);
        }
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
        bool isNoneRoom = GameManager.Instance.currentRoomType == RoomType.None;
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
        hpSlider.fillRect.gameObject.SetActive(true);
        GameManager.Instance.hero.inventory.InvStateDebug();

        moneyText.text = GameManager.Instance.hero.inventory.GetGold().ToString() + " gold";
        roomCountText.text = "Room num: " + GameManager.Instance.roomCount.ToString();
        hpText.text = "HP: " + GameManager.Instance.hero.currHP.ToString() + "/" + GameManager.Instance.hero.currMaxHP.ToString();

        int hpPerc = GameManager.Instance.hero.GetHpPerc();
        hpSlider.value = hpPerc;

        if(hpPerc > 50) {
            stateIcon.sprite = stateIconHp1;
        } else if (hpPerc > 25) {
            stateIcon.sprite = stateIconHp2;
        } else if (GameManager.Instance.hero.IsAlive()) {
            stateIcon.sprite = stateIconHp3;
        } else {
            stateIcon.sprite = stateIconHp4;
            hpSlider.fillRect.gameObject.SetActive(false);
        }

        for(int i = 0; i < 9; i++) {
            inventorySlots[i].UpdateSprite();
        }

        activeEffectsUI.Refresh();
    }

    public void ShowInfobox(Item item, RectTransform slotRect, int index) {
        HideInfobox();
        int offset = 0;
        if (index >= 6 && index < 9) {
            offset = -560;
        }
        RectTransform infoboxRect = infobox.GetComponent<RectTransform>();
        infoboxRect.anchoredPosition = slotRect.anchoredPosition + new Vector2(-355 + offset, 250);

        infoBoxName.text = item.nameStr;
        infoBoxDesc.text = item.description;
        infoBoxType.text = item.itemTypes;

        infoboxRect.gameObject.SetActive(true);
    }

    public void HideInfobox() {
        infobox.gameObject.SetActive(false);
    }

    public void ShowGameOver() {
        gameOverScreen.gameObject.SetActive(true);
    }

    public void ClickedTryAgain() {
        GameManager.Instance.Retry();
    }

    public void HideGameOver() {
        gameOverScreen.gameObject.SetActive(false);
    }

}
