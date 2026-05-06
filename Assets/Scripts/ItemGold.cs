/**
 * 
 */

using UnityEngine;

public class ItemGold : Item {

    

    public void InitGold(int value) { //1-30 rooms
        Debug.Log("attempting gold spawn");

        if (value < 10) {
            InitializeFromDB(ItemName.GoldCoin);
        } else if (value > 20) {
            InitializeFromDB(ItemName.GoldPile);
        } else {
            InitializeFromDB(ItemName.GoldBag);
        }
        this.value = value;
    }

    void OnMouseDown() {
        GameManager.Instance.addToInventory(this);
        Destroy(gameObject);
    }

}
