/**
 * Hero logic/data class
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Hero : Creature {


    public Inventory inventory = new Inventory();

    public void InitializeFromDB(HeroType heroType) {
        if (HeroDB.heroes.TryGetValue(heroType, out var heroData)) {

            nameStr = heroType.ToString();
            maxHP = heroData.maxHP;
            currHP = maxHP;
            minDMG = heroData.minDMG;
            maxDMG = heroData.maxDMG;
            dodge = heroData.dodge;
            acc = heroData.acc;

            def = 0;

            ResetToBaseStats();
            
            Debug.Log($"Spawned {nameStr} with {currHP}/{maxHP} HP");
        } else {
            Debug.LogError($"Hero '{heroType}' not found in database!");
        }
    }

    public void Heal(int amount) {
        currHP += amount;
        if (currHP > maxHP) {
            currHP = maxHP;
        }
        UIManager.Instance.RefreshHUD();
    }

    public void RecalculateStats() {
        ResetToBaseStats();
        Debug.Log("recalculating");
        inventory.UseEffectOnly(7);
        Debug.Log("counted ring 1");
        inventory.UseEffectOnly(8);
        Debug.Log("counted ring2");
        //active buffs/debuffs?
    }

}