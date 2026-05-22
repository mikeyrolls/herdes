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
	public int maxHP;
    public int currHP;
    public int gold;

	protected int minDMG;
	protected int maxDMG;
	protected int dodge;
	protected int acc;
    protected int def = 0;

    // -------------------------[ methods ]-----------------------------------------

    public bool LandHit() {
        return Helper.GetPerc() <= acc;
    }

    public void Miss() {
        sceneObject.PlayAnimation(AnimationType.Attack);
        // "missed"?
    }

    public int Attack() {
        sceneObject.PlayAnimation(AnimationType.Attack);
        return GiveDmg();
    }

    public bool GetAttacked(int dmg) {  //todo change to void once text set up
        if(Dodge()) {
            sceneObject.PlayAnimation(AnimationType.Dodge);
            return false;
            //dodged
        } else {
            TakeDmg(dmg);
            
            
            //dmg
            if(IsAlive()) {
                sceneObject.PlayAnimation(AnimationType.GetHurt);
            } else {
                sceneObject.PlayAnimation(AnimationType.Death);
            }
            return true;
        }
    }

    // smaller methods

    public bool IsAlive() {
        return (currHP > 0);
    }

    protected void TakeDmg(int rawDmg) {
        currHP -= rawDmg;
        if (currHP < 0) {
            currHP = 0;
        }
        UIManager.Instance.RefreshHUD();
    }

    protected int GiveDmg() {
        int rawDmg = Random.Range(minDMG, maxDMG);
        return rawDmg;
    }

    protected bool Dodge() {
        return Helper.GetPerc() <= dodge;
    }



    public int GetHpPerc() {
        return 100 * currHP / maxHP;
    }

    public int GetGoldValue() {
        return gold;
    }

}