/**
 * Enemy logic/data class
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Enemy : Creature {

    public EnemyType enemyType;
	
	public ItemName dropItem;
	int dropRate;
    public int gold;

    public bool isMimic = false;

    public void InitEnemy(EnemyType enemyType) {
        this.enemyType = enemyType;
        if (enemyType == EnemyType.Mimic) {
            InitializeMimic();
        } else {
            InitializeFromDB(enemyType);    
        }
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

    public void InitializeMimic() {
            Hero hero = GameManager.Instance.hero;
            isMimic = true;

            nameStr = enemyType.ToString();

            switch(hero.heroType) {
                case HeroType.Fishbone: 
                default: 
                    enemyType = EnemyType.MimicFishbone; break;
            }
            

            maxHP = hero.currMaxHP;
            currHP = maxHP;
            minDMG = hero.currMinDMG;
            maxDMG = hero.currMaxDMG;
            dodge = hero.currDodge;
            acc = hero.currAcc;
            gold = 0;
            dropItem = ItemName.None;
            dropRate = 0;
            def = hero.currDef;

            ResetToBaseStats();
            LoadEffects();

            Debug.Log($"Spawned {nameStr} with {currHP}/{maxHP} HP");
    }

    protected override void LoadEffects() {
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

        if (isMimic) {
            if (Helper.GetPerc() < 33)
                return GameManager.Instance.hero.specialType;
            else 
                return 0;
        }


        switch(enemyType) {
            case EnemyType.Slime:
            case EnemyType.Rat:
            case EnemyType.MimicFishbone:
                if(Helper.GetPerc() < 33 && GetHpPerc() < 70) return 1;
                break;
            case EnemyType.Bandit:
            case EnemyType.Spider:
            case EnemyType.Golem:
                if(Helper.GetPerc() < 25) return 2;
                break;
        }
        return 0;
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


