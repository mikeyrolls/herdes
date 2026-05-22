/**
 * Enemy game object class
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class EnemyGO : CreatureGO {

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
}
