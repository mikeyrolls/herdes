/**
 * Inherit for Hero and Enemy classes
 */

using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Creature : MonoBehaviour {

    public String nameStr;
	public int maxHP;
    public int currHP;
    public int gold;

	protected int minDMG;
	protected int maxDMG;
	protected int dodge;
	protected int acc;
    protected int def = 0;

    public bool IsAlive() {
        return (currHP > 0);
    }

    public void TakeDmg(int rawDmg) {
        currHP -= rawDmg;
        if (currHP < 0) {
            currHP = 0;
        }
        UIManager.Instance.RefreshHUD();
    }

    public int GiveDmg() {
        int rawDmg = Random.Range(minDMG, maxDMG);
        return rawDmg;
    }

    public int GetHpPerc() {
        return 100 * currHP / maxHP;
    }

}