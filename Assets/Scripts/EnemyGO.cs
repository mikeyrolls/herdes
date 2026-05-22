/**
 * Enemy game object class
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class EnemyGO : CreatureGO {

    public GameObject hpBarGreen;
    public GameObject hpBarRed;

    public override void InitGO(FightSpriteSet set) {
        base.InitGO(set);
        SetIsEnemy(true);
    }

    void OnMouseDown() {
        Debug.Log("enemy clicked");
        FindAnyObjectByType<HallwayManager>().OnEnemyClicked();
    }

    void OnMouseEnter() {
        UIManager.Instance.CursorSetHoverEnemy();
    }

    void OnMouseExit() {
        UIManager.Instance.CursorSetDefault();
    }

    public void SetHpBar(float percent) {
        StartCoroutine(SetHpBarAnim(percent/100));
        if(percent == 0) {
            StartCoroutine(HideHpBarAnim());
        }
    }

    public IEnumerator SetHpBarAnim(float size) {
        yield return new WaitForSeconds(animSpeed/2);
        hpBarGreen.transform.localScale = new Vector3(size, 1, 1);
        hpBarGreen.transform.localPosition = new Vector3(-(1 - size)/2, 0, 0);
    }

    public IEnumerator HideHpBarAnim() {
        yield return new WaitForSeconds(animSpeed/2);
        hpBarRed.SetActive(false);
    }
}
