/**
 * Hero logic/data class
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Hero : Creature {

    // hub scene object

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

}