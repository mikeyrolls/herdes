/**
 * Inventory logic/data class
 */

using UnityEngine;

public class Inventory {

    private int inventorySize = 6;
    public Item[] itemInventory  = new Item[10];     // indexes 0-5 normal
                                                    //         6 weapon
                                                    //         7-8 rings
    private int gold = 1000;

    private int GetEmptyInvSlot() {
        int i = 0;
        for(; i < inventorySize; i++) {
            if (itemInventory[i] == null) break;
        }
        Debug.Log("empty slot on " + i);
        return i;
    }

    public bool addToInventory(Item item) {
        
        if (item.itemType == ItemType.Gold) {
            IncreaseGold(item.value);
        } else {
            int firstEmpty = GetEmptyInvSlot();
            if (firstEmpty < inventorySize) {
                itemInventory[firstEmpty] = item;
                Debug.Log("added " + item.nameStr + " on inv space " + firstEmpty); 
            } else {
                return false;
            }
        }
        UIManager.Instance.RefreshHUD();
        return true;
    }

    public void IncreaseGold(int added) {
        gold += added;
        UIManager.Instance.RefreshHUD();
    }

    public bool DecreaseGold(int taken) {
        if (taken > gold) return false;

        gold -= taken;
        UIManager.Instance.RefreshHUD();
        return true;
    }

    public int GetGold() {
        return gold;
    }

    public bool Buy(Item item) {
        Debug.Log("buying " + item.nameStr);
        if(item.price > GetGold()) return false;

        if(addToInventory(item)) {
            DecreaseGold(item.price);
            return true;
        }
        return false;
    }

    public void UseFromInventory(int index) { 
        if (itemInventory[index] == null) return;

        if (itemInventory[index].IsConsumable()) {  //use consumables
            itemInventory[index].UseItem();
            itemInventory[index] = null;
        } else if (itemInventory[index].itemType == ItemType.Ring) {    // find slot for ring
            if(itemInventory[7] == null || itemInventory[8] != null) {  // first empty or both taken
                Swap(index, 7);
            } else {    //first taken second empty
                Swap(index, 8);
            }
        }

        UIManager.Instance.RefreshHUD();
    }

    public void UnequipFromInventory(int index) {
        if (itemInventory[index] == null) return;

        int invIndex = GetEmptyInvSlot();
        if (invIndex < 6) {
            Swap(index, invIndex);
        }
        UIManager.Instance.RefreshHUD();
    }

    public void InvStateDebug() {
        string log = "";
        int i = 0;
        for(; i < inventorySize; i++) {
            log += "[" + i + ": ";
            if (itemInventory[i] != null) log += itemInventory[i].nameStr;
            log += "] ";
        }
        Debug.Log(log);
    }

    public bool Swap(int index1, int index2) {  //holding 1, moving to 2

        if (index2 == 9) itemInventory[index1] = null;  //trashed

        Item item1 = itemInventory[index1];
        if (!CanItemBePlaced(item1, index2)) return false;
        Item item2 = itemInventory[index2];
        if (!CanItemBePlaced(item2, index1)) return false;
        
        itemInventory[index2] = item1;
        itemInventory[index1] = item2;
        GameManager.Instance.hero.RecalculateStats();
        UIManager.Instance.RefreshHUD();
        return true;
    }

    public bool CanItemBePlaced(Item item, int index) {
        if (item == null) return true;
        if (index < 6) return true;
        if (index == 7 || index == 8) return item.itemType == ItemType.Ring;
        return false;
    }

    public void UseEffectOnly(int index) {
        if (itemInventory[index] == null) return;
        itemInventory[index].UseItem();
    }

}