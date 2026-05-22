/**
 * Enemy game object class
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class EnemyGO : CreatureGO {

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

    public void Kill() {
        UIManager.Instance.CursorSetDefault();
        Destroy(gameObject);
        //todo loot?
    }



    // public void ShowDamagedSprite() {
    //     sr.sprite = enemySpriteSet.damaged;
    // }

    public void ShowDeadSprite() {
        sr.sprite = enemySpriteSet.dead;
    }

    public void ShowIdleSprite() {
        sr.sprite = enemySpriteSet.idle;
    }

    public override IEnumerator DodgeAnimation() {  // no color change
        yield return StartCoroutine(MoveSprite(Direction.Right));
        yield return StartCoroutine(MoveSprite(Direction.Left));
    }

    public override IEnumerator GetHurtAnimation() {  // red tint up
        yield return StartCoroutine(MoveSprite(Direction.Right, 0.75f));
        yield return StartCoroutine(MoveSprite(Direction.Left, 0));
    }

}
