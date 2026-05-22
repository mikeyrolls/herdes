/**
 * Hero game object class
 */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class HeroGO : CreatureGO {

    public override IEnumerator DodgeAnimation() {
        // yield return StartCoroutine(MoveSprite(Direction.Left));
        // yield return StartCoroutine(MoveSprite(Direction.Right));
        yield return null;
    }

    public override IEnumerator GetHurtAnimation() {
        // yield return StartCoroutine(MoveSprite(Direction.Left));
        // yield return StartCoroutine(MoveSprite(Direction.Right));
        yield return null;
    }

}