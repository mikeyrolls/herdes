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
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider hpSliderEnemy;
    [SerializeField] TMP_Text hpTextEnemy;

    [SerializeField] InventorySlot[] inventorySlots = new InventorySlot[10];

    [SerializeField] private ActiveEffectsUI activeEffectsUI;
    [SerializeField] private ActiveEffectsUI activeEffectsUIEnemy;

    [SerializeField] GameObject infobox;
    [SerializeField] TMP_Text infoBoxName;
    [SerializeField] TMP_Text infoBoxDesc;
    [SerializeField] TMP_Text infoBoxType;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject screenBlock;


    void Start() {
        GameManager.Instance.LoadScene("Room_Between");
    }

    public void Init() {
        hpSlider.minValue = 0f;
        hpSlider.maxValue = 100f;
        hpSliderEnemy.minValue = 0f;
        hpSliderEnemy.maxValue = 100f;

        hpSliderEnemy.gameObject.SetActive(false);

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
        FindAnyObjectByType<HubRoomManager>()?.RotateLeft();
    }

    public void OnRotateRight() {
        FindAnyObjectByType<HubRoomManager>()?.RotateRight();
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

        moneyText.text = GameManager.Instance.hero.inventory.GetGold().ToString() + " gold";
        roomCountText.text = GameManager.Instance.roomCount.ToString();
        hpText.text = GameManager.Instance.hero.currHP.ToString() + "/" + GameManager.Instance.hero.currMaxHP.ToString();

        float width = 50f + 60f * Mathf.Sqrt(GameManager.Instance.hero.currMaxHP);
        ((RectTransform)hpSlider.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

        int hpPerc = GameManager.Instance.hero.GetHpPerc();
        hpSlider.value = hpPerc;

        stateIcon.sprite = ((HeroSpriteSet)GameManager.Instance.hero.spriteSet).StateIcon(hpPerc);
        if(!GameManager.Instance.hero.IsAlive()) {
            hpSlider.fillRect.gameObject.SetActive(false);
        }

        for(int i = 0; i < 9; i++) {
            inventorySlots[i].UpdateSprite();
        }

        activeEffectsUI.Refresh(GameManager.Instance.hero);

        if(GameManager.Instance.combat) RefreshEnemy(GameManager.Instance.currEnemy);
    }

    public void RefreshEnemy(Enemy enemy) {

        hpTextEnemy.text = enemy.currHP.ToString() + "/" + enemy.currMaxHP.ToString();

        float width = 50f + 60f * Mathf.Sqrt(enemy.currMaxHP);
        ((RectTransform)hpSliderEnemy.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        hpSliderEnemy.value = enemy.GetHpPerc();

        if(!enemy.IsAlive()) {
            hpSliderEnemy.fillRect.gameObject.SetActive(false);
        }

        activeEffectsUIEnemy.Refresh(enemy);
    }

    public void StartingCombat() {
        hpSliderEnemy.gameObject.SetActive(true);
        hpSliderEnemy.fillRect.gameObject.SetActive(true);
    }

    public void EndingCombat() {
        hpSliderEnemy.gameObject.SetActive(false);
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

    public IEnumerator ShowGameOver() {
        gameOverScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GameManager.Instance.SetDefaultSprites();

        float duration = 1f;
        float elapsed = 0f;
        while (elapsed < duration) {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            gameOverMenu.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(0f, 800f), Vector2.zero, t);
            yield return null;
        }
    }

    public void ClickedTryAgain() {
        gameOverMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 800f);
        GameManager.Instance.Retry();
    }

    public IEnumerator BlockScreenFor(float seconds) {
        screenBlock.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        screenBlock.gameObject.SetActive(false);
    }

    public void HideGameOver() {
        gameOverScreen.gameObject.SetActive(false);
    }

}
