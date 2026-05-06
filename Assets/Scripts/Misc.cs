/**
 * Helper methods/classes, enums, databases
 */

using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;


// -----------[ helpers ]-----------

public enum Direction{Left, Middle, Right, Back}

public enum SpriteColor{Normal, Red}

public static class Helper {

    public static int roomAmount;

    static Helper () {
        roomAmount = Enum.GetNames(typeof(Room)).Length;
    }

    public static int GetPerc() {
        return Random.Range(0, 100);
    }

}


// -----------[ roomgen ]-----------

public enum Room{None, Back, Shop, Wall, Fight, Treasure } //fakewall, fishing, ...


// -----------[ item ]-----------

public enum ItemName {
    GoldCoin, GoldPile, GoldBag,    // gold
    Carrot, Drumstick, HealingPotion, LargeHealingPotion, HealingGland,   //heals

    //DodgePotion, None   //other
}

public enum ItemType{Gold, Heal, Buff, Upgrade, Story, Other}

public static class ItemDB {
    public static Dictionary<ItemName, ( string displayName,  int price, int value, ItemType itemType)> items = new() {
        //name                              name                            price   value
        [ItemName.GoldCoin] =               ("gold coin",                   0,      5,     ItemType.Gold),
        [ItemName.GoldBag] =                ("gold bag",                    0,      15,     ItemType.Gold),
        [ItemName.GoldPile] =               ("gold pile",                   0,      25,     ItemType.Gold),

        [ItemName.Carrot] =                 ("carrot",                      10,      5,     ItemType.Heal),
        [ItemName.Drumstick] =              ("drumstick",                   20,      10,     ItemType.Heal),
        [ItemName.HealingPotion] =          ("healing potion",              35,      15,     ItemType.Heal),
        [ItemName.LargeHealingPotion] =     ("large healing potion",        50,      20,     ItemType.Heal),
        [ItemName.HealingGland] =           ("healing gland",               100,     50,     ItemType.Heal),

    };
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


// -----------[ enemy ]-----------

public enum EnemyType {   
    //Slime,  Goblin,     
    Bat,    
    //Bandit,     Spider,     Undead,     Skeleton,   Snail,      Troll
}

public static class EnemyDB {

    public static Dictionary<EnemyType, ( int maxHP, int minDMG, int maxDMG, int dodge, int acc, int gold, ItemName dropItem, int dropRate)> enemies = new() {
        
        // name                     hp      dmg     dodg    acc     gold    drop + rate
        // [EnemyType.Slime] =         (20,    2, 5,   15,     70,     5,      ItemName.HealingPotion, 70),
        // [EnemyType.Goblin] =        (15,    5, 10,  30,     75,     15,     ItemName.None, 0),
        [EnemyType.Bat] =           (15,    1, 10,  40,     90,     10,     ItemName.Carrot, 35),
        // [EnemyType.Bandit] =        (35,    10, 12, 10,     70,     50,     ItemName.None, 0),    //weapon sharpen
        // [EnemyType.Spider] =        (20,    10, 15, 40,     90,     100,    ItemName.None, 0)     //big healing potion
        // [EnemyType.Undead] =        (25,    1, 5,   10,     60,     5,      ItemName.None,        0),
        // [EnemyType.Skeleton] =      (20,    5, 7,   15,     65,     10,     ItemName.None,        0),
        // [EnemyType.GiantSnail] =    (40,    1, 2,   0,      100,    15,     ItemName.None,        0)  //armor/armor potion
    };
}