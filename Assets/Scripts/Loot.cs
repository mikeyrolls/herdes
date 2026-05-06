/**
 * Loot game object and script object
 */

using UnityEngine;

public class Loot : MonoBehaviour {
    public string itemName;
    
    public LootType lootType = LootType.Other;
    public int value;

    public Loot(LootPreset preset) {
        itemName = preset.ToString();
        value = 0;
    }

    void OnMouseDown() {
        GameManager.Instance.addToInventory(this);
        Destroy(gameObject);
    }

    public void Init(LootType setLootType, int setValue, string setItemName) {
        lootType = setLootType;
        value = setValue;
        itemName = setItemName;
    }

    //todo better init
    //todo sprite system how??

}


