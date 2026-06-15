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

    // -------------------------[ stats ]-----------------------------------------

    public String nameStr;
    public int currHP;

    protected int maxHP;
	protected int minDMG;
	protected int maxDMG;
	protected int dodge;
	protected int acc;
    protected int def;

    public int currMaxHP;
    protected int currMinDMG;
    protected int currMaxDMG;
    protected int currDodge;
    protected int currAcc;
    protected int currDef;

    List<int> numbers = new List<int> {};

    // -------------------------[ methods ]-----------------------------------------

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

    public bool GetAttacked(int dmg) {  //todo change to void once text set up
        if(Dodge()) {
            sceneObject.PlayAnimation(AnimationType.Dodge);
            sceneObject.ShowFloatingText("Dodged", FloatingTextType.Miss);  
            return false;
        } else {
            int actuallyTaken = TakeDmg(dmg);
            sceneObject.ShowFloatingText(actuallyTaken + "", FloatingTextType.Hit);

            if(IsAlive()) {
                sceneObject.PlayAnimation(AnimationType.GetHurt);
            } else {
                sceneObject.PlayAnimation(AnimationType.Death);
            }
            return true;
        }
    }

    public void TakePoisonDmg(int rawDmg) {
        currHP = Helper.AddPositive(currHP, -rawDmg);
        UIManager.Instance.RefreshHUD();
        //todo anim
    }

    public void HealBuff(int amount) {
        Heal(amount);
        //todo anim
    }

    public void Heal(int amount) {
        currHP += amount;
        if (currHP > currMaxHP) {
            currHP = currMaxHP;
        }
        UIManager.Instance.RefreshHUD();
    }

    // smaller methods

    public bool IsAlive() {
        return (currHP > 0);
    }

    protected virtual int TakeDmg(int rawDmg) {
        int armoredDmg = (int)(StatToMult(def) * rawDmg + 0.5);
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
        Debug.Log("increase stat temporary");
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

    public virtual void RecalculateStats() {
        
    }

    double StatToMult(int stat) { //decimal, 0 -> 1
        return ((1)/(1+0.02*stat));
    }

}