/**
 * Enemy game object and script object
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Enemy : Creature {

    private EnemyType enemyType;
	
	ItemName dropItem;
	int dropRate;

    public EnemySpriteSet enemySpriteSet;
    private SpriteRenderer sr;

    public void InitEnemy() {
        setEnemyType();
        InitializeFromDB(enemyType);
        sr.sprite = enemySpriteSet.idle;
        sr.sortingOrder = 10;
        mat = GetComponent<SpriteRenderer>().material;
        Debug.Log("sprite and mat for enemy set");
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
        Debug.Log("enemy clicked");
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
        mat = GetComponent<SpriteRenderer>().material;
    }

    public void ShowDamagedSprite() {
        sr.sprite = enemySpriteSet.damaged;
    }

    public void ShowIdleSprite() {
        sr.sprite = enemySpriteSet.idle;
    }

    // ------------------------------------------------------------------

    public override IEnumerator DodgeAnimation() {  // no color change
        yield return StartCoroutine(MoveSprite(Direction.Right));
        yield return StartCoroutine(MoveSprite(Direction.Left));
    }

    public override IEnumerator GetHurtAnimation() {  // red tint up
        yield return StartCoroutine(MoveSprite(Direction.Right, 0.75f));
        yield return StartCoroutine(MoveSprite(Direction.Left, 0));
    }

}
