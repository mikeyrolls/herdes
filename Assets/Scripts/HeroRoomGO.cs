/**
 * Hero game object class for hub room and maybe shop?
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class HeroRoomGO : CreatureGO, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public RoomSpriteSet roomSpriteSet;
    public bool isAnimated;

    public override void InitGO(FightSpriteSet set) { return; }

    public void InitGO(RoomSpriteSet set) {
        isEnemy = false;
        roomSpriteSet = set;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = roomSpriteSet.idle;
    }

    public void OnPointerClick(PointerEventData e) {
        if (!isAnimated) {
            StartCoroutine(Pet());
        }
    }

    public void OnPointerEnter(PointerEventData e) {
        if(!isAnimated)
            CursorManager.Instance.AddRequest(this, CursorType.Grab, Prio.World, "");
    }

    public void OnPointerExit(PointerEventData e) {
        CursorManager.Instance.RemoveRequest(this);
    }

    public IEnumerator Pet() {
        isAnimated = true;
        sr.sprite = roomSpriteSet.pet;
        yield return new WaitForSeconds(animSpeed/2);
        sr.sprite = roomSpriteSet.idle;
        isAnimated = false;
    }

    public override void SetDamagedSprite() {
        sr.sprite = roomSpriteSet.damaged;
    }

    public override void SetDeadSprite() {
        sr.sprite = roomSpriteSet.pet;
    }
    
}