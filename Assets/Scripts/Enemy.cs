/**
 * Enemy game object and script object
 */

using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Enemy : Creature {

    private EnemyType enemyType;
	
	LootPreset dropItem;
	int dropRate;

    public EnemySpriteSet enemySpriteSet;
    private SpriteRenderer sr;

    public void InitEnemy() {
        setEnemyType();
        InitializeFromDB(enemyType);
        sr.sprite = enemySpriteSet.idle;
        sr.sortingOrder = 10;
    }

    public void InitEnemy(EnemyType enemyType) {
        this.enemyType = enemyType;
        InitializeFromDB(enemyType);
        sr = GetComponent<SpriteRenderer>(); // just force it here for now
        sr.sprite = enemySpriteSet.idle;
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

    void OnMouseDown() {
        FindAnyObjectByType<HallwayManager>().OnEnemyClicked();
    }

    void OnMouseEnter() {
        UIManager.Instance.CursorSetHoverEnemy();
    }

    void OnMouseExit() {
        UIManager.Instance.CursorSetDefault();
    }

    public void Kill() {
        UIManager.Instance.CursorSetDefault();
        Destroy(gameObject);
        //todo loot?
    }

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ShowDamagedSprite() {
        sr.sprite = enemySpriteSet.damaged;
    }

    public void ShowIdleSprite() {
        sr.sprite = enemySpriteSet.idle;
    }

}
