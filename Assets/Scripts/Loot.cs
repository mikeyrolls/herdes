using UnityEngine;

public enum LootPreset {
    DodgePotion, None
}

public class Loot : MonoBehaviour
{
    public string itemName;
    public enum LootType{gold, heal, armor, other}
    
    public LootType lootType = LootType.other;
    public int value;

    public Loot(LootPreset preset) {
        itemName = preset.ToString();
        value = 0;
    }


    void OnMouseDown()
    {
        GameManager.Instance.addToInventory(this);
        Destroy(gameObject);
    }

    public void Init(LootType setLootType, int setValue, string setItemName) {
        lootType = setLootType;
        value = setValue;
        itemName = setItemName;
    }


}
