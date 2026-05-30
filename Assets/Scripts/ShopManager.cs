/**
 * Handles shop room
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour {

    [SerializeField] ShopSlot[] shopSlots = new ShopSlot[4];
    Hero hero;

    void Start() {

        Debug.Log("starting shop");
        hero = GameManager.Instance.hero;

        foreach (var slot in shopSlots)
            slot.Init(ItemName.Drumstick);
        
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
        if(hero.buy(slot.item)) {
            slot.SetSoldOut();
            DisableExpensive();
        }
        
    }

    private void DisableExpensive() {
        foreach (var slot in shopSlots)
            slot.DisableExpensive(hero.gold);
    }


}
