/**
 * Inherit for Hero and Enemy logic/data classes
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Creature {

    public CreatureGO sceneObject;
    public FightSpriteSet spriteSet;

    // -------------------------[ stats ]-----------------------------------------

    public String nameStr;
    public int currHP;

    protected int maxHP;
	protected int minDMG;
	protected int maxDMG;
	protected int dodge;
	protected int acc;
    public int def; //todo back

    public int currMaxHP;
    public int currMinDMG;
    public int currMaxDMG;
    public int currDodge;
    public int currAcc;
    public int currDef;

    public EffectList effectList;


    public List<(EffectName effect, int duraction, int value, int perc)>[] attackEffects = new List<(EffectName effect, int duraction, int value, int perc)>[] {
        new List<(EffectName, int, int, int)>(),  //dealt in normal attack
        new List<(EffectName, int, int, int)>(),  //special fast
        new List<(EffectName, int, int, int)>()   //special charged
    };

    public bool isCharging = false;

    // -------------------------[ methods ]-----------------------------------------

    public Creature() {
        effectList = new EffectList(this);
    }

    public void ResetToBaseStats() {
        currMaxHP = maxHP;
        currMinDMG = minDMG;
        currMaxDMG = maxDMG;
        currDodge = dodge;
        currAcc = acc;
        currDef = def;
    }

    public bool LandHit() {
        return Helper.GetPerc() <= currAcc;
    }

    public void Miss() {
        sceneObject.PlayAnimation(AnimationType.Attack);
        sceneObject.ShowFloatingText("Missed", FloatingTextType.Miss);  
    }

    public int Attack() {
        sceneObject.PlayAnimation(AnimationType.Attack);
        return GiveDmg();
    }

    public bool GetAttacked(int dmg) {  //todo change to void once text set up??? idk
        if(Dodge()) {
            sceneObject.PlayAnimation(AnimationType.Dodge);
            sceneObject.ShowFloatingText("Dodged", FloatingTextType.Miss);  
            return false;
        } else {
            int actuallyTaken = TakeDmg(dmg);
            sceneObject.ShowFloatingText(actuallyTaken + "", FloatingTextType.Hit);
            sceneObject.PlayAnimation(AnimationType.GetHurt);

            return true;
        }
    }

    public virtual void DoSpecialFast(string text = "", FloatingTextType type = FloatingTextType.Default) {
        sceneObject.PlayAnimation(AnimationType.SpecialFast);
        if (text != "")
            sceneObject.ShowFloatingText(text, type);
    }

    public void DoSpecialCharging() {
        sceneObject.SetChargingIdle();
        sceneObject.SetIdleSprite();
        sceneObject.ShowFloatingText("Charging", FloatingTextType.Default);
        isCharging = true;
    }

    public int DoSpecialCharged() {
        sceneObject.PlayAnimation(AnimationType.SpecialCharged);
        sceneObject.SetNormalIdle();
        isCharging = false;
        return 2 * GiveDmg();
    }

    public void TakeDotDmg(int rawDmg, FloatingTextType type) {
        currHP = Helper.AddPositive(currHP, -rawDmg);
        UIManager.Instance.RefreshHUD();
        sceneObject.ShowFloatingText(rawDmg + "", type);
        if (GameManager.Instance.currentRoomType != RoomType.Fight)
            sceneObject.PlayDotAnimation();
        if(!IsAlive())
            sceneObject.DieGO(); //animates and invokes death
    }

    public virtual void Heal(int amount) {

        if (currHP + amount > currMaxHP) {
            amount = currMaxHP - currHP;
        }
        currHP += amount;
        sceneObject.ShowFloatingText(amount + "", FloatingTextType.Heal); 

        UIManager.Instance.RefreshHUD();
    }

    // smaller methods

    public bool IsAlive() {
        return (currHP > 0);
    }

    protected virtual int TakeDmg(int rawDmg) {
        int armoredDmg = (int)(StatToMult(currDef) * rawDmg + 0.5);
        currHP = Helper.AddPositive(currHP, -armoredDmg);
        UIManager.Instance.RefreshHUD();
        return armoredDmg;
    }

    protected int GiveDmg() {
        int rawDmg = Random.Range(currMinDMG, currMaxDMG);
        return rawDmg;
    }

    protected bool Dodge() {
        return Helper.GetPerc() <= currDodge;
    }

    public int GetHpPerc() {
        return 100 * currHP / currMaxHP;
    }

    public void IncreaseStatPermanent(int amount, StatType stat) {
        switch(stat) {  //MaxHP, DMG, Dodge, Acc, Def, 
            case StatType.MaxHP:
                maxHP = Helper.AddPositive(maxHP, amount);
                break;
            case StatType.DMG:
                minDMG = Helper.AddPositive(minDMG, amount);
                maxDMG = Helper.AddPositive(maxDMG, amount);
                break;
            case StatType.Dodge:
                dodge = Helper.AddPositive(dodge, amount);
                break;
            case StatType.Acc:
                acc = Helper.AddPositive(acc, amount);
                break;
            case StatType.Def:
                def = Helper.AddPositive(def, amount);
                break;
        }
        RecalculateStats();
        UIManager.Instance.RefreshHUD();
    }

    public void IncreaseStatTemporary(int amount, StatType stat) {
        switch(stat) {  //MaxHP, DMG, Dodge, Acc, Def, 
            case StatType.MaxHP:
                currMaxHP = Helper.AddPositive(currMaxHP, amount);
                break;
            case StatType.DMG:
                currMinDMG = Helper.AddPositive(currMinDMG, amount);
                currMaxDMG = Helper.AddPositive(currMaxDMG, amount);
                break;
            case StatType.Dodge:
                currDodge = Helper.AddPositive(currDodge, amount);
                break;
            case StatType.Acc:
                currAcc = Helper.AddPositive(currAcc, amount);
                break;
            case StatType.Def:
                currDef = Helper.AddPositive(currDef, amount);
                break;
        }
        UIManager.Instance.RefreshHUD();
    }

    double StatToMult(int stat) { //decimal, 0 -> 1
        return ((1)/(1+0.02*stat));
    }

    public List<(EffectName effect, int duration, int value)> GetEffectsFromAtkType(int type) {

        var e = new List<(EffectName, int, int)>();
        foreach (var x in attackEffects[type]) {
            if (Helper.GetPerc() <= x.perc) {
                e.Add((x.effect, x.duraction, x.value));
            } 
        }
        return e;
    }

    public virtual int HealAfterAttack(int attackType) {
        return 0;
    }

    protected virtual void LoadEffects() { }

    public virtual void Die() {
        sceneObject.DieGO();
        // you can notify things here todo
    }

    public void ApplyEffects() {
        effectList.ApplyEffectsFromList();
    }

    public void RecalculateStats() {
        ResetToBaseStats();
        effectList.CalculateEffects();
        if (currHP > currMaxHP) currHP = currMaxHP;
        UIManager.Instance.RefreshHUD();
    }

}