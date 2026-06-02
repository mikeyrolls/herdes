/**
 * Helper methods/classes, enums, databases
 */

using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using System.Linq;


// -----------[ helpers ]-----------

public enum Direction{Left, Middle, Right, Back}

public enum Duration{Temporary, Permanent}

public enum SpriteColor{Normal, Red}

public enum CursorType {Normal, Attack, Grab}

public enum Prio {Base, Background, World, UI, Overlay}

public static class Helper {

    public static int GetPerc() {
        return Random.Range(0, 100);
    }

    public static int AddPositive(int a, int b) {
        int r = a + b;
        return (r >= 0) ? r : 0;
    }
}


// -----------[ roomgen ]-----------
      
public enum RoomType{None, Back, Back2, Shop,          // special
                Wall,                       // never middle
                Empty, Fight, Treasure, Fakewall } 
                //fishing, ...

public class Room {
    public RoomType roomType;
    public Sprite roomSprite;

    public void ChangeRoomType(RoomType newRoomType, Sprite newRoomSprite) {
        roomType = newRoomType;
        roomSprite = newRoomSprite;
    }
}


// -----------[ item ]-----------

public enum ItemName {
    GoldCoin, GoldPile, GoldBag,    // gold
    Carrot, Drumstick, HealingPotion, LargeHealingPotion, HealingGland,   //heals
    HealthUpgrade, DmgUpgrade,      // upgrades
    AttackRing, DodgeRing,

    Poison, ContinuousHeal,
    //DodgePotion, None   //other
    None,
}

public enum ItemType{Gold, Heal, Ring, Upgrade, Buff,
                    //Story, Other
                    }

public enum ItemRarity { Common, Uncommon, Rare, Special }

public static class ItemDB {
    public static Dictionary<ItemName, ( string displayName,  int price, int value, ItemType itemType, ItemRarity rarity, string description)> items = new() {
        //name                              name                            price   value
        [ItemName.GoldCoin] =               ("Gold coin",                   0,      5,     ItemType.Gold, ItemRarity.Common,   ""),
        [ItemName.GoldBag] =                ("Gold bag",                    0,      15,     ItemType.Gold, ItemRarity.Common,  ""),
        [ItemName.GoldPile] =               ("Gold pile",                   0,      25,     ItemType.Gold, ItemRarity.Uncommon,  ""),

        //                                                                          heals for
        [ItemName.Carrot] =                 ("Carrot",                      10,     5,     ItemType.Heal, ItemRarity.Common,   "A small carrot. Heals for 5 HP."),
        [ItemName.Drumstick] =              ("Drumstick",                   20,     10,     ItemType.Heal, ItemRarity.Common,  "A juicy drumstick. Heals for 10 HP."),
        [ItemName.HealingPotion] =          ("Healing potion",              35,     15,     ItemType.Heal, ItemRarity.Uncommon,  "An average looking healing potion. Heals for 15 HP."),
        [ItemName.LargeHealingPotion] =     ("Large healing potion",        50,     20,     ItemType.Heal, ItemRarity.Uncommon,  "An extra large healing potion. Heals for 12 HP."),
        [ItemName.HealingGland] =           ("Healing gland",               100,    100,     ItemType.Heal, ItemRarity.Rare, "A squishy spider gland. A heal for the bravest."),

        //                                                                          stat increase
        [ItemName.HealthUpgrade] =          ("Health upgrade",              5,     2,     ItemType.Upgrade, ItemRarity.Uncommon,   "Permanently upgrades hp by 1."),
        [ItemName.DmgUpgrade] =             ("Dmg upgrade",                 7,     1,     ItemType.Upgrade, ItemRarity.Uncommon,  "Permanently upgrades atk by 1."),
        

        //                                                                          increase stat for
        [ItemName.AttackRing] =             ("Ring of attack",              10,     50,     ItemType.Ring, ItemRarity.Common,   "Hydrogen bomb."),
        [ItemName.DodgeRing] =              ("Ring of dodge",               20,     100,     ItemType.Ring, ItemRarity.Common,  "Don't get hit ever."),

        [ItemName.Poison] =                 ("Poison",                      20,     0,     ItemType.Buff, ItemRarity.Common,  "Get poisoned."),
        [ItemName.ContinuousHeal] =         ("Continuous heal",             20,     0,     ItemType.Buff, ItemRarity.Common,  "Get healed slowly."),

    };

    public static List<ItemName> GetPool(ItemRarity rarity) {
        return items.Where(kvp => kvp.Value.rarity == rarity).Select(kvp => kvp.Key).ToList();
    }

    public static List<ItemName> GetShopPool(ItemRarity rarity) {
        return items.Where(kvp => kvp.Value.rarity == rarity && kvp.Value.itemType != ItemType.Gold).Select(kvp => kvp.Key).ToList();
    }

    public static ItemName GetRandom(int roomNumber) {
        ItemRarity rarity = GetRarity(roomNumber);
        var pool = GetPool(rarity);
        return pool[Random.Range(0, pool.Count)];
    }

    public static ItemName GetRandomForShop(int roomNumber) {
        ItemRarity rarity = GetRarity(roomNumber);
        var pool = GetShopPool(rarity);
        return pool[Random.Range(0, pool.Count)];
    }

    public static ItemRarity GetRarity(int roomNumber) {
        float perc = Helper.GetPerc();
        float mult = (float)(1 / (1 + 0.02 * roomNumber) + 0.25);
        perc *= mult;

        if (perc < 15) { //rare items 10%
            return ItemRarity.Rare;
        } else if (perc < 45) { //uncommon items 35%
            return ItemRarity.Uncommon;
        } else { //common items 55% 
            return ItemRarity.Common;
        } 
    }
}




// -----------[ hero ]-----------

public enum HeroType {   
    Fishbone,   // default, hero, cannot flee?
    Clover,      // fuck if I know man
    Tacobean    //throws bombs

    // mage can heal, archer thief can go "invisible" (buff dodge, maybe crit dodge?) from old
}

public static class HeroDB {
    public static Dictionary<HeroType, ( int maxHP, int minDMG, int maxDMG, int dodge, int acc )> heroes = new() {
        //name                  hp      dmg     dodge   acc
        [HeroType.Fishbone] =   (40,    8, 10,  10,     100),

    };
}

public enum StatType {
    MaxHP, DMG, Dodge, Acc, Def, CurrHP
}


// -----------[ enemy ]-----------

public enum EnemyType {   
    Slime, Bat, Bandit,     Spider,  Snail,      Golem,
    Flies, Rat, Snake,

    Fishbone,
}

public static class EnemyDB {

    public static Dictionary<EnemyType, ( int maxHP, int minDMG, int maxDMG, int dodge, int acc, int gold, ItemName dropItem, int dropRate)> enemies = new() {
        
        // name                     hp      dmg     dodg    acc     gold    drop + rate
        [EnemyType.Slime] =         (20,    2, 5,   15,     70,     5,      ItemName.HealingPotion, 70),
        // [EnemyType.Goblin] =        (15,    5, 10,  30,     75,     15,     ItemName.None, 0),
        [EnemyType.Bat] =           (15,    1, 10,  40,     90,     15,     ItemName.Carrot, 25),
        [EnemyType.Bandit] =        (35,    10, 12, 10,     70,     50,     ItemName.None, 0),    //weapon sharpen
        [EnemyType.Spider] =        (20,    10, 15, 40,     90,     100,    ItemName.HealingGland, 100),     //big healing potion
        // [EnemyType.Undead] =        (25,    1, 5,   10,     60,     5,      ItemName.None,        0),
        // [EnemyType.Skeleton] =      (20,    5, 7,   15,     65,     10,     ItemName.None,        0),
        [EnemyType.Snail] =         (40,    1, 2,   0,      100,    10,     ItemName.None,        0),  //armor/armor potion

        [EnemyType.Flies] =         (10,    2, 6,   0,      50,    3,     ItemName.None,        0),
        [EnemyType.Rat] =           (15,    2, 5,   20,      100,    10,     ItemName.None,        0),
        [EnemyType.Snake] =         (20,    5, 10,   20,      90,    25,     ItemName.None,        0),
        [EnemyType.Golem] =         (50,    8, 10,   5,      65,    50,     ItemName.None,        0),
    };

    public static Dictionary<EnemyType, FightSpriteSet> sprites = new();

    public static void InitSprites(Dictionary<EnemyType, FightSpriteSet> spriteSets) {
        sprites = spriteSets;
    }

    private static readonly (EnemyType enemy, int unlockAtRoom)[] EnemyUnlocks = {
        (EnemyType.Slime,  0),
        (EnemyType.Snail,  0),
        (EnemyType.Flies,  0),
        (EnemyType.Rat,    0),
        (EnemyType.Bat,    5),
        (EnemyType.Snake,  10),
        (EnemyType.Bandit, 15),
        (EnemyType.Golem,  20),
        (EnemyType.Spider, 25),
    };

    public static EnemyType GetRandomEnemy(int roomNumber) {
        List<EnemyType> pool = new List<EnemyType>();

        foreach (var (enemy, unlockAtRoom) in EnemyUnlocks)
            if (roomNumber >= unlockAtRoom)
                pool.Add(enemy);
        return pool[Random.Range(0, pool.Count)];
    }
}

// -----------[ combat ]-----------

public enum AnimationType {   
    Attack, Dodge, GetHurt, Death
}


// -----------[ effects ]-----------

public enum EffectName {
    HpInc, AtkInc, AccInc, DodgeInc, DefInc,
    HpDec, AtkDec, AccDec, DodgeDec, DefDec,
    Heal, Poison, PoisonBig,
}

public static class EffectDB {

    public static Dictionary<EffectName, ( int duration, bool isDebuff, StatType stat, int value)> effects = new() {
        // name                     duration    is debuff   stat                value
        [EffectName.HpInc] =        (10,        false,      StatType.MaxHP,    5),
        [EffectName.AtkInc] =       (10,        false,      StatType.DMG,    1),
        [EffectName.AccInc] =       (10,        false,      StatType.Acc,    10),
        [EffectName.DodgeInc] =     (10,        false,      StatType.Dodge,    10),
        [EffectName.DefInc] =       (10,        false,      StatType.Def,    10),

        [EffectName.HpDec] =        (5,        true,      StatType.MaxHP,    5),
        [EffectName.AtkDec] =       (5,        true,      StatType.DMG,    1),
        [EffectName.AccDec] =       (5,        true,      StatType.Acc,    10),
        [EffectName.DodgeDec] =     (5,        true,      StatType.Dodge,    10),
        [EffectName.DefDec] =       (5,        true,      StatType.Def,    10),

        [EffectName.Heal] =         (5,        false,      StatType.CurrHP,    4),
        [EffectName.Poison] =       (5,        true,      StatType.CurrHP,    2),
        [EffectName.PoisonBig] =    (5,        true,      StatType.CurrHP,    5),
    };


}