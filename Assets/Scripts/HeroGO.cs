/**
 * Hero game object class
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class HeroGO : CreatureGO {

    public override void InitGO(FightSpriteSet set) {
        base.InitGO(set);
        SetIsEnemy(false);
    }
    
}