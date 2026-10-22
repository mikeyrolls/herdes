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

    public bool isAnimated;
    public HeroSpriteSet heroSpriteSet;

    public override void InitGO(FightSpriteSet set) {
        isEnemy = false;
        fightSpriteSet = set;
        sr = GetComponent<SpriteRenderer>();
        heroSpriteSet = (HeroSpriteSet)fightSpriteSet;

        sr.sprite = heroSpriteSet.roomIdle;
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
        sr.sprite = heroSpriteSet.pet;
        yield return new WaitForSeconds(animSpeed);
        sr.sprite = heroSpriteSet.roomIdle;
        isAnimated = false;
    }

    public override void SetDamagedSprite() {
        sr.sprite = heroSpriteSet.roomDamaged;
    }

    public override void SetDeadSprite() {
        sr.sprite = heroSpriteSet.roomDead;
    }

    public override void PlayDotAnimation() {
        if (isDotAnimating) return;
        StartCoroutine(PoisonRoomAnimation());
    }

    IEnumerator PoisonRoomAnimation() {
        isDotAnimating = true;
        sr.sprite = heroSpriteSet.roomDamaged;
        yield return new WaitForSeconds(animSpeed);
        sr.sprite = heroSpriteSet.roomIdle;
        isDotAnimating = false;
    }
    
}