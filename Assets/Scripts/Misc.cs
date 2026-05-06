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


// -----------[ loot ]-----------

public enum LootPreset {
    DodgePotion, None
}

public enum LootType{Gold, Heal, Armor, Other}


// -----------[ hero ]-----------

public enum HeroType {   
    Fishbone,   // default, hero, cannot flee?
    Clover,      // fuck if I know man
    Tacobean    //throws bombs

    // mage can heal, archer thief can go "invisible" (buff dodge, maybe crit dodge?) from old
}

public static class HeroDB {
    public static Dictionary<HeroType, ( int maxHP, int minDMG, int maxDMG, int dodge, int acc )> enemies = new() {
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

    public static Dictionary<EnemyType, ( int maxHP, int minDMG, int maxDMG, int dodge, int acc, int gold, LootPreset dropItem, int dropRate)> enemies = new() {
        
        // name                     hp      dmg     dodg    acc     gold    drop + rate
        // [EnemyType.Slime] =         (20,    2, 5,   15,     70,     5,      LootPreset.HealingPotion, 70),
        // [EnemyType.Goblin] =        (15,    5, 10,  30,     75,     15,     LootPreset.None, 0),
        [EnemyType.Bat] =           (150,    1, 10,  40,     90,     10,     LootPreset.DodgePotion, 35),
        // [EnemyType.Bandit] =        (35,    10, 12, 10,     70,     50,     LootPreset.None, 0),    //weapon sharpen
        // [EnemyType.Spider] =        (20,    10, 15, 40,     90,     100,    LootPreset.None, 0)     //big healing potion
        // [EnemyType.Undead] =        (25,    1, 5,   10,     60,     5,      LootPreset.None,        0),
        // [EnemyType.Skeleton] =      (20,    5, 7,   15,     65,     10,     LootPreset.None,        0),
        // [EnemyType.GiantSnail] =    (40,    1, 2,   0,      100,    15,     LootPreset.None,        0)  //armor/armor potion
    };
}