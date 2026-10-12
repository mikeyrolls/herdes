/**
 * Hero game object class for hub room and maybe shop?
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class HeroRoomGO : CreatureGO {

    public RoomSpriteSet roomSpriteSet;
    public bool isAnimated;

    public override void InitGO(FightSpriteSet set) { return; }

    public void InitGO(RoomSpriteSet set) {
        roomSpriteSet = set;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = roomSpriteSet.idle;
    }

    void OnMouseDown() {
        Debug.Log("hero clicked in room");
        if (!isAnimated) {
            StartCoroutine(Pet());
        }
    }

    void OnMouseEnter() {
        if(!isAnimated)
            CursorManager.Instance.AddRequest(this, CursorType.Grab, Prio.World, "");
    }

    void OnMouseExit() {
        CursorManager.Instance.RemoveRequest(this);
    }

    public IEnumerator Pet() {
        isAnimated = true;
        sr.sprite = roomSpriteSet.pet;
        yield return new WaitForSeconds(animSpeed/2);
        sr.sprite = roomSpriteSet.idle;
        isAnimated = false;
    }
    
}