/**
 * Enemy logic/data class
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Enemy : Creature {

    //public EnemyGO sceneObject;

    private EnemyType enemyType;
	
	ItemName dropItem;
	int dropRate;

    //???

    // public void InitEnemy() {
    //     setEnemyType();
    //     InitializeFromDB(enemyType);
    //     sr.sprite = enemySpriteSet.idle;
    //     sr.sortingOrder = 10;
    //     mat = GetComponent<SpriteRenderer>().material;
    //     Debug.Log("sprite and mat for enemy set");
    // }

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
            dropItem = enemyData.dropItem;
            dropRate = enemyData.dropRate;
            def = 0;
            
            Debug.Log($"Spawned {nameStr} with {currHP}/{maxHP} HP");
        } else {
            Debug.LogError($"Enemy type '{enemyType}' not found in database!");
        }
    }

}
