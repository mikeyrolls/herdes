/**
 * Hero game object class
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;

public class HeroGO : CreatureGO, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler  {

    public override void InitGO(FightSpriteSet set) {
        base.InitGO(set);
        SetIsEnemy(false);
    }

    public void OnPointerClick(PointerEventData e) {
        FindAnyObjectByType<HallwayManager>().OnHeroClicked();
    }

    public void OnPointerEnter(PointerEventData e) {
        CursorManager.Instance.AddRequest(this, CursorType.Special, Prio.World, "");
    }

    public void OnPointerExit(PointerEventData e) {
        CursorManager.Instance.RemoveRequest(this);
    }
    
}