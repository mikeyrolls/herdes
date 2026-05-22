/**
 * Inherit for Hero and Enemy game object classes
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class CreatureGO : MonoBehaviour {

    // -------------------------[ object vars ]-----------------------------------------

    public EnemySpriteSet enemySpriteSet;
    protected SpriteRenderer sr;

    protected Material mat;
    protected float animSpeed = 0.25f;
    protected float animDist = 0.4f;

    // -------------------------[ object methods ]-----------------------------------------

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        mat = GetComponent<SpriteRenderer>().material;
    }

    public void InitVisuals(EnemySpriteSet set) {
        sr = GetComponent<SpriteRenderer>();
        enemySpriteSet = set;
        sr.sprite = enemySpriteSet.idle;
    }

    protected IEnumerator MoveSprite(Direction direction, float tintStrength = 0) {
        return MoveSprite(direction, tintStrength, Color.red);
    }

    protected IEnumerator MoveSprite(Direction direction, float tintStrength, Color tintColor) {
        float distance = animDist;
        float duration = animSpeed/2;
        if(direction == Direction.Left) distance *= -1;

        // movement
        float elapsed = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(distance, 0f, 0f);

        // color
        float startTintStrength = mat.GetFloat("_TintStrength");
        Color startTintColor = mat.GetColor("_TintColor");
        mat.SetColor("_TintColor", tintColor);
        float tintVector = tintStrength - startTintStrength;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            if (tintVector != 0) {
                mat.SetFloat("_TintStrength", startTintStrength + tintVector*t);
            }
            yield return null;
        }

        transform.position = targetPosition;
        mat.SetFloat("_TintStrength", tintStrength);
    }

    public virtual IEnumerator DodgeAnimation() {
        yield return null;
    }

    public virtual IEnumerator GetHurtAnimation() {  // just white
        yield return null;
    }

}