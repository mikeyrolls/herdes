/**
 * Hero logic/data class
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Hero : Creature {

    HeroType heroType;

    public String specialDescription;
    public int specialType;

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

            specialDescription = heroData.specialDescription;
            specialType = heroData.specialType;

            this.heroType = heroType;
            def = 0;

            ResetToBaseStats();
            LoadEffects();
            
            Debug.Log($"Spawned {nameStr} with {currHP}/{maxHP} HP");
        } else {
            Debug.LogError($"Hero '{heroType}' not found in database!");
        }
    }

    // public override void DoSpecialFast(string text = "", FloatingTextType type = FloatingTextType.Default) {
    //     if(heroType == HeroType.Fishbone)
    //         base.DoSpecialFast("Blocking");
    // }

    protected override void LoadEffects() {
        switch(heroType) {
            //effect duration value perc
            case HeroType.Fishbone:
                attackEffects[1].Add((EffectName.DefInc, 1, 100, 100)); break;
            // case HeroType.Ghost:
            //     attackEffects[2].Add((EffectName.DodgeInc, 5, 50, 100)); break;

        }
    }

}