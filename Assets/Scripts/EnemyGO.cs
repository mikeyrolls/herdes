/**
 * Enemy game object class
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class EnemyGO : CreatureGO, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public GameObject hpBarGreen;
    public GameObject hpBarRed;

    bool hovered;
    bool cursorActive;
    HallwayManager hallway;

    void Update() {
        if (hovered) 
            RefreshCursor();
    }

    public override void InitGO(FightSpriteSet set) {
        base.InitGO(set);
        SetIsEnemy(true);
    }

    public void OnPointerClick(PointerEventData e) {
        FindAnyObjectByType<HallwayManager>().OnEnemyClicked();
    }

    public void OnPointerEnter(PointerEventData e) {
        hovered = true;
        RefreshCursor();
    }

    public void OnPointerExit(PointerEventData e) {
        hovered = false;
        RefreshCursor();
    }

    void OnDeath() {
        CursorManager.Instance.RemoveRequest(this);
    }

    void RefreshCursor() {
        if (hallway == null) hallway = FindAnyObjectByType<HallwayManager>();
        bool wantAttack = hovered && hallway.IsPlayerTurn;

        if (wantAttack && !cursorActive) {
            CursorManager.Instance.AddRequest(this, CursorType.Attack, Prio.World, "attack");
            cursorActive = true;
        } else if (!wantAttack && cursorActive) {
            CursorManager.Instance.RemoveRequest(this);
            cursorActive = false;
        }
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

    public void SetHpBarWidth(int maxHP) {
        float width = 0.4f + Mathf.Sqrt(maxHP / 4);
        hpBarRed.transform.localScale = new Vector3(width, 0.4f, 1);
    }

    public IEnumerator HideHpBarAnim() {
        yield return new WaitForSeconds(animSpeed/2);
        hpBarRed.SetActive(false);
    }
}
