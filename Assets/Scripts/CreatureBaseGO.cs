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

    public FightSpriteSet fightSpriteSet;
    protected SpriteRenderer sr;

    protected Material mat;
    protected float animSpeed = 0.45f; //0.25f;
    protected float animDist = 1.4f;

    protected Direction dirCenter;
    protected Direction dirEdge;

    public Action onDeath;

    // -------------------------[ object methods ]-----------------------------------------

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        mat = GetComponent<SpriteRenderer>().material;
    }

    public virtual void InitGO(FightSpriteSet set) {
        sr = GetComponent<SpriteRenderer>();
        fightSpriteSet = set;
        sr.sprite = fightSpriteSet.idle;
    }

    protected void SetIsEnemy(bool isEnemy) {
        dirCenter = (isEnemy) ? Direction.Left : Direction.Right;
        dirEdge = (isEnemy) ? Direction.Right : Direction.Left;
    }

    protected IEnumerator FadeSprite(float tintStrength = 0) {
        return MoveSprite(Direction.Right, tintStrength, Color.red, 0);
    }

    protected IEnumerator MoveSprite(Direction direction, float tintStrength = 0) {
        return MoveSprite(direction, tintStrength, Color.red, animDist);
    }

    protected IEnumerator MoveSprite(Direction direction, float tintStrength, Color tintColor, float distance) {
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

    public void PlayAnimation(AnimationType a) {
        switch(a) {
            case AnimationType.Attack:
                StartCoroutine(AttackAnimation());
                break;
            case AnimationType.Dodge:
                StartCoroutine(DodgeAnimation());
                break;
            case AnimationType.GetHurt:
                StartCoroutine(GetHurtAnimation());
                break;
            case AnimationType.Death:
                StartCoroutine(DeathAnimation());
                break;
        }
    }

    public IEnumerator DodgeAnimation() {  // dodge immediatelly
        yield return StartCoroutine(MoveSprite(dirEdge));
        yield return StartCoroutine(MoveSprite(dirCenter));
    }

    public IEnumerator GetHurtAnimation() {  // red tint up, move after hit
        yield return new WaitForSeconds(animSpeed/2);
        SetDamagedSprite();
        yield return StartCoroutine(MoveSprite(dirEdge, 0.75f));
        yield return StartCoroutine(MoveSprite(dirCenter, 0));
        SetIdleSprite();
    }

    public IEnumerator AttackAnimation() {  // move towards
        SetAttackingSprite();
        yield return StartCoroutine(MoveSprite(dirCenter));
        yield return StartCoroutine(MoveSprite(dirEdge));
        SetIdleSprite();
    }

    public IEnumerator DeathAnimation() {
        yield return new WaitForSeconds(animSpeed/2);
        SetDamagedSprite();
        yield return StartCoroutine(MoveSprite(dirEdge, 0.75f));
        onDeath?.Invoke();
        GetComponent<Collider2D>().enabled = false;
        SetDeadSprite();
        UIManager.Instance.CursorSetDefault();
        yield return StartCoroutine(FadeSprite(0f));
        // yield return new WaitForSeconds(2f);
        // Destroy(gameObject);
    }

    public void SetIdleSprite() { //todo change protected
        sr.sprite = fightSpriteSet.idle;
    }

    public void SetDamagedSprite() {
        sr.sprite = fightSpriteSet.damaged;
    }

    public void SetAttackingSprite() {
        sr.sprite = fightSpriteSet.attacking;
    }

    public void SetDeadSprite() {
        sr.sprite = fightSpriteSet.dead;
    }
}