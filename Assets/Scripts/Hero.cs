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
    public EffectList effectList = new EffectList();

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

    public override void RecalculateStats() {
        Debug.Log("recalculating, maxhp " + maxHP + ", currmaxhp " + currMaxHP + ", currhp " + currHP);
        ResetToBaseStats();
        
        inventory.UseEffectOnly(7);
        inventory.UseEffectOnly(8);
        effectList.CalculateEffects();
        Debug.Log("recalculating pre scale, maxhp " + maxHP + ", currmaxhp " + currMaxHP + ", currhp " + currHP);
        if (currHP > currMaxHP) currHP = currMaxHP;
        Debug.Log("recalculating done, maxhp " + maxHP + ", currmaxhp " + currMaxHP + ", currhp " + currHP);
    }

}