/**
 * Single effect in char info
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CharInfoEffectRow : MonoBehaviour {
    [SerializeField] Image icon;
    [SerializeField] TMP_Text text;
    [SerializeField] EffectSpriteDB effectSpriteDB;

    public void Init(Effect effect) {
        icon.sprite = effectSpriteDB.Get(effect.name);
        text.text = effect.received;

        if(effect.stat == StatType.CurrHP) {
            if (effect.isDebuff) 
                text.text += " -";
            else 
                text.text += " +";
            text.text += effect.value + "hp";
        } else {
            if (effect.isDebuff) 
                text.text += " -";
            else 
                text.text += " +";
            text.text += effect.value;
        }

        if (effect.duration > 0)
            text.text += " (" + effect.duration + " turns left)";
    }
}