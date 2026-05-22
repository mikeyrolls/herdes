/**
 * Item sprite dictionary (todo change to dict and name lol)
 */

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class ItemSpriteDatabase : ScriptableObject {
    public List<ItemSpriteEntry> entries;
    
    public Sprite GetSprite(ItemName id) {
        foreach (var entry in entries) {
            if (entry.id == id) {
                return entry.sprite;
            }
        }
        Debug.LogWarning($"No sprite found for id: {id}");
        return null;
    }
}

[System.Serializable]
public class ItemSpriteEntry {
    public ItemName id;
    public Sprite sprite;
}