/**
 * Hero game object and script object
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Hero : Creature {

    public void InitializeFromDB(HeroType heroType) {
        if (HeroDB.heroes.TryGetValue(heroType, out var heroData)) {

            nameStr = heroType.ToString();
            maxHP = heroData.maxHP;
            currHP = maxHP;
            minDMG = heroData.minDMG;
            maxDMG = heroData.maxDMG;
            dodge = heroData.dodge;
            acc = heroData.acc;

            gold = 0;
            def = 0;
            
            Debug.Log($"Spawned {nameStr} with {currHP}/{maxHP} HP");
        } else {
            Debug.LogError($"Hero '{heroType}' not found in database!");
        }
    }

    public bool updateMoney(int amount) {
        if (gold + amount < 0) {
            return false;
        } else {
            gold += amount;
            return true;
        }

    }

    public override IEnumerator DodgeAnimation() {
        // yield return StartCoroutine(MoveSprite(Direction.Left));
        // yield return StartCoroutine(MoveSprite(Direction.Right));
        yield return null;
    }

    public override IEnumerator GetHurtAnimation() {
        // yield return StartCoroutine(MoveSprite(Direction.Left));
        // yield return StartCoroutine(MoveSprite(Direction.Right));
        yield return null;
    }

}