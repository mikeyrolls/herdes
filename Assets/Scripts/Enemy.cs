/**
 * Enemy game object and script object
 */

using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Enemy : Creature {

    private EnemyType enemyType;
	
	Loot dropItem;
	int dropRate;

    [SerializeField] private Sprite spriteNormal;

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

            nameStr = enemyType.ToString();
            maxHP = enemyData.maxHP;
            currHP = maxHP;
            minDMG = enemyData.minDMG;
            maxDMG = enemyData.maxDMG;
            dodge = enemyData.dodge;
            acc = enemyData.acc;
            gold = enemyData.gold;
            dropItem = new Loot(enemyData.dropItem);
            dropRate = enemyData.dropRate;
            def = 0;
            
            Debug.Log($"Spawned {nameStr} with {currHP}/{maxHP} HP");
        } else {
            Debug.LogError($"Enemy type '{enemyType}' not found in database!");
        }
    }

    void OnMouseDown()
    {

        FindObjectOfType<HallwayManager>().OnEnemyClicked();

        // hurt for 1hp now
        GameManager.Instance.hero.TakeDmg(1);
        
        if(!IsAlive()) {
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

}
