/**
 * Item sprite dictionary (todo change to dict and name lol)
 */

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemSpriteDB", menuName = "Game/Item Sprite DB")]
public class ItemSpriteDB : ScriptableObject {

    public Sprite goldCoin;
    public Sprite goldPile;
    public Sprite goldBag;

    public Sprite carrot;
    public Sprite drumstick;
    public Sprite healingPotion;
    public Sprite largeHealingPotion;
    public Sprite healingGland;

    public Sprite healthUpgrade;
    public Sprite dmgUpgrade;

    public Sprite poison;
    public Sprite continuousHealSmall;
    public Sprite continuousHeal;
    public Sprite continuousHealLarge;
    public Sprite attackPotion;
    public Sprite clearingPotion;

    public Sprite healthRingSmall;
    public Sprite healthRing;
    public Sprite healthRingLarge;
    public Sprite atkRing;
    public Sprite atkRingLarge;
    public Sprite defRing;
    public Sprite defRingLarge;
    public Sprite dodgeRing;
    public Sprite dodgeRingLarge;
    public Sprite regenRing;

    private Dictionary<ItemName, Sprite> itemSpritesDict;

    public Dictionary<ItemName, Sprite> ItemSpritesDict {
        get {
            if (itemSpritesDict == null) BuildItemSpritesDict();
            return itemSpritesDict;
        }
    }

    private void BuildItemSpritesDict() {
        itemSpritesDict = new Dictionary<ItemName, Sprite> {

            [ItemName.GoldCoin] = goldCoin,
            [ItemName.GoldPile] = goldPile,
            [ItemName.GoldBag] = goldBag,

            [ItemName.Carrot] = carrot,
            [ItemName.Drumstick] = drumstick,
            [ItemName.HealingPotion] = healingPotion,
            [ItemName.LargeHealingPotion] = largeHealingPotion,
            [ItemName.HealingGland] = healingGland,

            [ItemName.HealthUpgrade] = healthUpgrade,
            [ItemName.DmgUpgrade] = dmgUpgrade,

            [ItemName.Poison] = poison,
            [ItemName.ContinuousHealSmall] = continuousHealSmall,
            [ItemName.ContinuousHeal] = continuousHeal,
            [ItemName.ContinuousHealLarge] = continuousHealLarge,
            [ItemName.AttackPotion] = attackPotion,
            [ItemName.ClearingPotion] = clearingPotion,

            [ItemName.HealthRingSmall] = healthRingSmall,
            [ItemName.HealthRing] = healthRing,
            [ItemName.HealthRingLarge] = healthRingLarge,
            [ItemName.AtkRing] = atkRing,
            [ItemName.AtkRingLarge] = atkRingLarge,
            [ItemName.DefRing] = defRing,
            [ItemName.DefRingLarge] = defRingLarge,
            [ItemName.DodgeRing] = dodgeRing,
            [ItemName.DodgeRingLarge] = dodgeRingLarge,
            [ItemName.RegenRing] = regenRing,
        };
    }

    public Sprite Get(ItemName item) => ItemSpritesDict.TryGetValue(item, out var sprite) ? sprite : null;
}