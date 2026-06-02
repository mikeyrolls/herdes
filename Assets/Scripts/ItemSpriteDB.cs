/**
 * Item sprite dictionary (todo change to dict and name lol)
 */

using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemSpriteDB", menuName = "Game/Item Sprite DB")]
public class ItemSpriteDB : ScriptableObject {

    public Sprite goldCoin;
    public Sprite goldBag;
    public Sprite goldPile;

    public Sprite carrot;
    public Sprite drumstick;
    public Sprite healSmall;
    public Sprite healBig;
    public Sprite healGland;

    public Sprite healthUpgrade;
    public Sprite dmgUpgrade;

    public Sprite attackRing;
    public Sprite dodgeRing;


    public Sprite poison;
    public Sprite continuousHeal;

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
            [ItemName.GoldBag] = goldBag,
            [ItemName.GoldPile] = goldPile,

            [ItemName.Carrot] = carrot,
            [ItemName.Drumstick] = drumstick,
            [ItemName.HealingPotion] = healSmall,
            [ItemName.LargeHealingPotion] = healBig,
            [ItemName.HealingGland] = healGland,

            [ItemName.HealthUpgrade] = healthUpgrade,
            [ItemName.DmgUpgrade] = dmgUpgrade,

            [ItemName.AttackRing] = attackRing,
            [ItemName.DodgeRing] = dodgeRing,


            [ItemName.Poison] = poison,
            [ItemName.ContinuousHeal] = continuousHeal,
        };
    }

    public Sprite Get(ItemName item) => ItemSpritesDict.TryGetValue(item, out var sprite) ? sprite : null;
}