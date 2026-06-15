/**
 * Enemy logic/data class
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Enemy : Creature {

    private EnemyType enemyType;
	
	public ItemName dropItem;
	int dropRate;
    public int gold;

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
            def = enemyData.def;

            ResetToBaseStats();
            
            Debug.Log($"Spawned {nameStr} with {currHP}/{maxHP} HP");
        } else {
            Debug.LogError($"Enemy type '{enemyType}' not found in database!");
        }
    }

    protected override void TakeDmg(int rawDmg) {
        base.TakeDmg(rawDmg);
        ((EnemyGO)sceneObject).SetHpBar(GetHpPerc());
    }

    public ItemName GetDrop() {
        if (dropRate > Helper.GetPerc()) {
            return dropItem;
        }
        return ItemName.None;
    }

    public int GetGoldValue() {
        return gold;
    }

}
