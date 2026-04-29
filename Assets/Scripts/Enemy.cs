using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;


public enum EnemyType
{   
    //Slime,  Goblin,     
    Bat,    
    //Bandit,     Spider,     Undead,     Skeleton,   Snail,      Troll
}

public static class EnemyDB {
    public static Dictionary<EnemyType, ( int maxHP, int minDMG, int maxDMG, int dodge, int ACC, int gold, LootPreset dropItem, int dropRate)> enemies = new() {
        
        // name                     hp      dmg     dodg    acc     gold    drop + rate
        // [EnemyType.Slime] =         (20,    2, 5,   15,     70,     5,      LootPreset.HealingPotion, 70),
        // [EnemyType.Goblin] =        (15,    5, 10,  30,     75,     15,     LootPreset.None, 0),
        [EnemyType.Bat] =           (15,    1, 10,  40,     90,     10,     LootPreset.DodgePotion, 35),
        // [EnemyType.Bandit] =        (35,    10, 12, 10,     70,     50,     LootPreset.None, 0),    //weapon sharpen
        // [EnemyType.Spider] =        (20,    10, 15, 40,     90,     100,    LootPreset.None, 0)     //big healing potion
        // [EnemyType.Undead] =        (25,    1, 5,   10,     60,     5,      LootPreset.None,        0),
        // [EnemyType.Skeleton] =      (20,    5, 7,   15,     65,     10,     LootPreset.None,        0),
        // [EnemyType.GiantSnail] =    (40,    1, 2,   0,      100,    15,     LootPreset.None,        0)  //armor/armor potion
    };
}

public class Enemy : MonoBehaviour {

    private EnemyType enemyType;

    public string enemyName;
	public int maxHP;
    public int currHP;
	private int minDMG;
	private int maxDMG;
	private int dodge;
	private int ACC;
	private int gold;
	
	Loot dropItem;
	int dropRate;

    [SerializeField] private EnemySpriteSet spriteSet;


    public void InitEnemy() {
        setEnemyType();
        InitializeFromDB(enemyType);
    }

    public void InitEnemy(EnemyType enemyType) {
        this.enemyType = enemyType;
        InitializeFromDB(enemyType);
    }




    void setEnemyType(){
        enemyType = (EnemyType)Random.Range(0, 
            Enum.GetNames(typeof(EnemyType)).Length
        );
    }

    void InitializeFromDB(EnemyType enemyType) {
        if (EnemyDB.enemies.TryGetValue(enemyType, out var enemyData)) {

            enemyName = enemyType.ToString();
            maxHP = enemyData.maxHP;
            currHP = maxHP;
            minDMG = enemyData.minDMG;
            maxDMG = enemyData.maxDMG;
            dodge = enemyData.dodge;
            ACC = enemyData.ACC;
            gold = enemyData.gold;
            dropItem = new Loot(enemyData.dropItem);
            dropRate = enemyData.dropRate;
            
            Debug.Log($"Spawned {enemyName} with {currHP}/{maxHP} HP");
        } else {
            Debug.LogError($"Enemy type '{enemyType}' not found in database!");
        }
    }

    void OnMouseDown()
    {

        FindObjectOfType<HallwayManager>().OnEnemyClicked();

        // hurt for 1hp now
        GameManager.Instance.takeDmg(1);
        
        if(!isAlive()) {
            GameManager.Instance.combat = false;
            UIManager.Instance.CursorSetDefault();
            Destroy(gameObject);
        }
    }

    void OnMouseEnter() {
        UIManager.Instance.CursorSetHoverEnemy();
    }

    void OnMouseExit() {
        UIManager.Instance.CursorSetDefault();
    }

    public bool isAlive() {
        return (currHP > 0);
    }

    public void takeDmg(int rawDmg) {
        currHP -= rawDmg;
        if (currHP < 0) {
            currHP = 0;
        }
    }
}
