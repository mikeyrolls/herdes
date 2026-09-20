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
            LoadEffects();
            
            Debug.Log($"Spawned {nameStr} with {currHP}/{maxHP} HP");
        } else {
            Debug.LogError($"Enemy type '{enemyType}' not found in database!");
        }
    }

    void LoadEffects() {
        Debug.Log("Loading effects mm");
        switch(enemyType) {
            //effect duration value perc
            case EnemyType.Bat:
                attackEffects[0].Add((EffectName.Bleed, 3, 2, 50)); break;
            case EnemyType.Bandit:
                attackEffects[2].Add((EffectName.BleedBig, 5, 5, 100)); break;
            case EnemyType.Rat:
                attackEffects[0].Add((EffectName.Bleed, 3, 2, 40)); break;
            case EnemyType.Spider:
                attackEffects[0].Add((EffectName.Poison, 3, 2, 50));
                attackEffects[2].Add((EffectName.PoisonBig, 5, 5, 100)); break;
            case EnemyType.Flies:
                attackEffects[0].Add((EffectName.Poison, 3, 2, 50)); break;
            case EnemyType.Snake:
                attackEffects[0].Add((EffectName.Poison, 3, 2, 80));
                attackEffects[2].Add((EffectName.PoisonBig, 5, 5, 80)); break;
            case EnemyType.Golem:
                attackEffects[2].Add((EffectName.AccDec, 5, 20, 50));
                attackEffects[2].Add((EffectName.AtkDec, 5,  3, 50));
                attackEffects[2].Add((EffectName.DefDec, 5, 20, 50));
                attackEffects[2].Add((EffectName.DodgeDec, 5, 20, 50)); break;
        }
        Debug.Log("effects loaded for normal " + attackEffects[0]);

        // end of LoadEffects()
Debug.Log($"[{enemyType}] attackEffects[0].Count={attackEffects[0].Count}, [1]={attackEffects[1].Count}, [2]={attackEffects[2].Count}");
    }

    protected override int TakeDmg(int rawDmg) {
        int ad = base.TakeDmg(rawDmg);
        ((EnemyGO)sceneObject).SetHpBar(GetHpPerc());
        return ad;
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

    public int GetAttackType() {
        switch(enemyType) {
            case EnemyType.Bat:
                //whatever
                //break
            default:
                return 0;
        }
    }

    public override int HealAfterAttack(int attackType) {
        //normal
        if(attackType == 0 && (enemyType == EnemyType.Bat)) return 1;
        //special fast
        if(attackType == 1 && enemyType == EnemyType.Slime) return 10;
        if(attackType == 1 && enemyType == EnemyType.Rat) return 8;

        return 0;
    }

}
