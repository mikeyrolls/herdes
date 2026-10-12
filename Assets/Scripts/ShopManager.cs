/**
 * Handles shop room
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour {

    [SerializeField] ShopSlot[] shopSlots = new ShopSlot[4];
    [SerializeField] GameObject heroPrefab;
    HeroRoomGO heroGO;
    Hero hero;
    public RoomSpriteSet roomSpriteSet;

    void Start() {

        Debug.Log("starting shop");
        hero = GameManager.Instance.hero;

        heroGO = Instantiate(heroPrefab, new Vector3(-1f, -3f, 0f), Quaternion.identity).GetComponent<HeroRoomGO>();
        heroGO.InitGO(roomSpriteSet);
        hero.sceneObject = heroGO;

        hero.effectList.ApplyEffects();

        foreach (var slot in shopSlots)
            slot.Init(ItemDB.GetRandomForShop(GameManager.Instance.roomCount));
            
        DisableExpensive();


    }

    private void OnEnable() {
        foreach (var slot in shopSlots)
            slot.OnPurchaseClicked += HandlePurchase;
    }

    private void OnDisable() {
        foreach (var slot in shopSlots)
            slot.OnPurchaseClicked -= HandlePurchase;
    }

    private void HandlePurchase(ShopSlot slot) {
        if(hero.inventory.Buy(slot.item)) {
            slot.SetSoldOut();
            DisableExpensive();
        }
    }

    private void DisableExpensive() {
        foreach (var slot in shopSlots)
            slot.DisableExpensive(hero.inventory.GetGold());
    }

}
