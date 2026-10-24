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
}
