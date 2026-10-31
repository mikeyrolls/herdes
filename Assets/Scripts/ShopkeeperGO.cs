using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShopkeeperGO : MonoBehaviour {

    public Transform body;
    public Transform legs;

    float duration = 1.2f;

    float bodyRotA = 0f;
    float bodyRotB = -5f;
    float legsOffsetA = 2f;
    float legsOffsetB = 3f;

    public bool rocking = false;

    public IEnumerator RockChair() {
        rocking = true;
        Vector3 legsStart = legs.localPosition;
        float elapsed = 0f;

        while (true) {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.PingPong(elapsed / duration, 1f);
            t = Mathf.SmoothStep(0f, 1f, t);

            body.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(bodyRotA, bodyRotB, t));
            legs.localPosition = legsStart + new Vector3(Mathf.Lerp(legsOffsetA, legsOffsetB, t), 0f, 0f);

            yield return null;
        }
    }

    public void StopRockingChair() {
        rocking = false;
    }


}