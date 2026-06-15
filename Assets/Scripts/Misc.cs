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
    Poison, ContinuousHealSmall, ContinuousHeal, ContinuousHealLarge, AttackPotion, ClearingPotion,
    HealthRingSmall, HealthRing, HealthRingLarge, AtkRing, AtkRingLarge, DefRing, DefRingLarge, DodgeRing, DodgeRingLarge, RegenRing,
    
    None,
}

public enum ItemType{Gold, Heal, Ring, Upgrade, Buff, Charm
                    //Story, Other
                    }

public enum ItemRarity { Common, Uncommon, Rare, Special }

public static class ItemDB {
    public static Dictionary<ItemName, ( string displayName,  int price, int value, ItemType itemType, ItemRarity rarity, string description)> items = new() {
        //gold                              name                            price   value
        [ItemName.GoldCoin] =               ("Gold coin",                   0,      5,     ItemType.Gold, ItemRarity.Common,   ""),
        [ItemName.GoldBag] =                ("Gold bag",                    0,      15,     ItemType.Gold, ItemRarity.Common,  ""),
        [ItemName.GoldPile] =               ("Gold pile",                   0,      25,     ItemType.Gold, ItemRarity.Uncommon,  ""),

        //heals                                                                     heals for
        [ItemName.Carrot] =                 ("Carrot",                      10,     5,     ItemType.Heal, ItemRarity.Common,   "Heals for 5 HP."),
        [ItemName.Drumstick] =              ("Drumstick",                   20,     10,     ItemType.Heal, ItemRarity.Common,  "Heals for 10 HP."),
        [ItemName.HealingPotion] =          ("Healing potion",              35,     15,     ItemType.Heal, ItemRarity.Uncommon,  "Heals for 15 HP."),
        [ItemName.LargeHealingPotion] =     ("Large healing potion",        50,     20,     ItemType.Heal, ItemRarity.Uncommon,  "Heals for 20 HP."),
        [ItemName.HealingGland] =           ("Healing gland",               100,    100,     ItemType.Heal, ItemRarity.Rare, "A heal for the bravest."),

        //upgrades                                                                 stat increase
        [ItemName.HealthUpgrade] =          ("Health upgrade",              75,     2,     ItemType.Upgrade, ItemRarity.Rare,   "Permanently upgrades HP by 1."),
        [ItemName.DmgUpgrade] =             ("Dmg upgrade",                 75,     1,     ItemType.Upgrade, ItemRarity.Rare,  "Permanently upgrades ATK by 1."),
        
        //buffs                                                                     x for 5 turns
        [ItemName.Poison] =                 ("Poison",                      1,      0,     ItemType.Buff, ItemRarity.Common,  "Poisons you. Try it."),
        [ItemName.ContinuousHealSmall] =    ("Small potion of regeneration",20,     2,     ItemType.Buff, ItemRarity.Common,  "Heals for 10 HP slowly."),
        [ItemName.ContinuousHeal] =         ("Potion of regeneration",      45,     4,     ItemType.Buff, ItemRarity.Uncommon,  "Heals for 20 HP slowly."),
        [ItemName.ContinuousHealLarge] =    ("Large potion of regeneration",70,     6,     ItemType.Buff, ItemRarity.Rare,  "Heals for 30 HP slowly."),
        [ItemName.AttackPotion] =           ("Potion of strength",          20,     10,     ItemType.Buff, ItemRarity.Common,  "Increases attack temporarily."),
        [ItemName.ClearingPotion] =         ("Potion of clarity",           40,     0,     ItemType.Buff, ItemRarity.Uncommon,  "Removes debuffs."),


        //rings                                                                     increase stat for
        [ItemName.HealthRingSmall] =        ("Weak ring of life",           20,     2,     ItemType.Ring, ItemRarity.Common,   "Increases HP by 2."),
        [ItemName.HealthRing] =             ("Ring of life",                50,     10,     ItemType.Ring, ItemRarity.Uncommon,   "Increases HP by 10."),
        [ItemName.HealthRingLarge] =        ("Strong ring of life",         100,    20,     ItemType.Ring, ItemRarity.Rare,   "Increases HP by 20."),
        [ItemName.AtkRing] =                ("Ring of strength",            50,     1,     ItemType.Ring, ItemRarity.Uncommon,   "Increases attack."),
        [ItemName.AtkRingLarge] =           ("Strong ring of strength",     100,    3,     ItemType.Ring, ItemRarity.Rare,   "Increases attack greatly."),
        [ItemName.DefRing] =                ("Ring of defense",             50,     10,     ItemType.Ring, ItemRarity.Uncommon,   "Increases defense."),
        [ItemName.DefRingLarge] =           ("Strong ring of defense",      100,    30,     ItemType.Ring, ItemRarity.Rare,   "Increases defense greatly."),
        [ItemName.DodgeRing] =              ("Ring of dodging",             50,     10,     ItemType.Ring, ItemRarity.Uncommon,   "Increases dodge."),
        [ItemName.DodgeRingLarge] =         ("Strong ring of dodging",      100,    20,     ItemType.Ring, ItemRarity.Rare,   "Increases dodge greatly."),

        [ItemName.RegenRing] =              ("Ring of blooming life",       100,    1,     ItemType.Ring, ItemRarity.Rare,   "Restores HP slowly."),
        
        //charms
        // [ItemName.SpiderCharm] =            ("Poison",                      20,     0,     ItemType.Buff, ItemRarity.Common,  "Get poisoned."),
        // [ItemName.DragonLureCharm] =        ("Continuous heal",             20,     0,     ItemType.Buff, ItemRarity.Common,  "Get healed slowly."),

        
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

    public static Dictionary<EnemyType, ( int maxHP, int minDMG, int maxDMG, int dodge, int acc, int def, int gold, ItemName dropItem, int dropRate)> enemies = new() {
        
        // name                     hp      dmg     dodg    acc     def     gold    drop + rate
        [EnemyType.Slime] =         (20,    2, 5,   15,     80,     0,    5,      ItemName.ContinuousHeal, 50),
        // [EnemyType.Goblin] =        (15,    5, 10,  30,     75,          15,     ItemName.None, 0),
        [EnemyType.Bat] =           (15,    1, 10,  40,     90,     0,    15,     ItemName.DodgeRing, 35),  //dodgepotion
        [EnemyType.Bandit] =        (35,    10, 12, 10,     90,     30,    50,     ItemName.DmgUpgrade, 40),    //weapon sharpen
        [EnemyType.Spider] =        (20,    10, 15, 40,     95,     0,    100,    ItemName.HealingGland, 100),     //big healing potion
        // [EnemyType.Undead] =        (25,    1, 5,   10,     60,          5,      ItemName.None,        0),
        // [EnemyType.Skeleton] =      (20,    5, 7,   15,     65,          10,     ItemName.None,        0),
        [EnemyType.Snail] =         (20,    1, 2,   0,      100,    90,    10,     ItemName.DefRing,        0),  //armor/armor potion

        [EnemyType.Flies] =         (10,    2, 6,   0,      70,     -20,    3,     ItemName.Poison,        10),
        [EnemyType.Rat] =           (15,    2, 5,   20,      100,   0,    10,     ItemName.Drumstick,        20),
        [EnemyType.Snake] =         (20,    5, 10,   20,      90,   10,    25,     ItemName.Poison,        100),
        [EnemyType.Golem] =         (50,    8, 10,   5,      75,    30,    50,     ItemName.AttackPotion,        50),
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

public enum FloatingTextType {   
    Miss, Hit, Poison, Heal, Debuff
}


// -----------[ effects ]-----------

public enum EffectName {
    HpInc, AtkInc, AccInc, DodgeInc, DefInc,
    HpDec, AtkDec, AccDec, DodgeDec, DefDec,
    Heal, Poison, PoisonBig,
}

public static class EffectDB {

    public static Dictionary<EffectName, (bool isDebuff, StatType stat)> effects = new() {
        // name                     is debuff   stat
        [EffectName.HpInc] =        (false,      StatType.MaxHP),
        [EffectName.AtkInc] =       (false,      StatType.DMG),
        [EffectName.AccInc] =       (false,      StatType.Acc),
        [EffectName.DodgeInc] =     (false,      StatType.Dodge),
        [EffectName.DefInc] =       (false,      StatType.Def),

        [EffectName.HpDec] =        (true,      StatType.MaxHP),
        [EffectName.AtkDec] =       (true,      StatType.DMG),
        [EffectName.AccDec] =       (true,      StatType.Acc),
        [EffectName.DodgeDec] =     (true,      StatType.Dodge),
        [EffectName.DefDec] =       (true,      StatType.Def),

        [EffectName.Heal] =         (false,      StatType.CurrHP),
        [EffectName.Poison] =       (true,      StatType.CurrHP),
        [EffectName.PoisonBig] =    (true,      StatType.CurrHP),
    };


}