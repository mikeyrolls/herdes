/**
 * Inherit for Hero and Enemy classes
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Creature : MonoBehaviour {

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

    // -------------------------[ animation vars ]-----------------------------------------

    protected Material mat;
    protected float animSpeed = 0.25f;
    protected float animDist = 0.4f;


    // -------------------------[ methods ]-----------------------------------------

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

    public bool Dodge() {
        return Helper.GetPerc() <= dodge;
    }

    public bool LandHit() {
        return Helper.GetPerc() <= acc;
    }

    public int GetGoldValue() {
        return gold;
    }

    // -------------------------[ animation methods ]-----------------------------------------

    protected IEnumerator MoveSprite(Direction direction, float tintStrength = 0) {
        return MoveSprite(direction, tintStrength, Color.red);
    }

    protected IEnumerator MoveSprite(Direction direction, float tintStrength, Color tintColor) {
        float distance = animDist;
        float duration = animSpeed/2;
        if(direction == Direction.Left) distance *= -1;

        // movement
        float elapsed = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(distance, 0f, 0f);

        // color
        float startTintStrength = mat.GetFloat("_TintStrength");
        Color startTintColor = mat.GetColor("_TintColor");
        mat.SetColor("_TintColor", tintColor);
        float tintVector = tintStrength - startTintStrength;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            if (tintVector != 0) {
                mat.SetFloat("_TintStrength", startTintStrength + tintVector*t);
            }
            yield return null;
        }

        transform.position = targetPosition;
        mat.SetFloat("_TintStrength", tintStrength);
    }

    public virtual IEnumerator DodgeAnimation() {
        yield return null;
    }

    public virtual IEnumerator GetHurtAnimation() {  // just white
        yield return null;
    }

}