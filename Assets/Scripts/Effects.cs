/**
 * EffectList and Effect classes
 */

using System.Collections.Generic;
using UnityEngine;

public class EffectList {
    public List<Effect> effects = new List<Effect> {};
    private int currId = 0;

    public int LeftRingEffectId;
    public int RightRingEffectId;

    public void RemoveDebuffs() {
        effects.RemoveAll(x => x.isDebuff);
    }

    public void AddEffectDurationValue(EffectName effectName, int duration, int value, int index = -1) {
        DebugPrintList();

        Effect effect = new Effect(effectName, duration, value, ++currId);
        effects.Add(effect);
        GameManager.Instance.hero.RecalculateStats();

        if (index == 7) LeftRingEffectId = currId;
        if (index == 8) RightRingEffectId = currId;

        Debug.Log("added effect " + effectName.ToString() + " from index " + index);
        DebugPrintList();
    }

    public void DebugPrintList() {
        string s = "";
        foreach (var effect in effects) {
            s += ("[id:" + effect.id + ", eff:" + effect.name + "], ");
        }
        Debug.Log(s);
    }

    public void RemoveEffectAtIndex(int index) {
        DebugPrintList();

        int id = 0;
        if (index == 7) id = LeftRingEffectId;
        if (index == 8) id = RightRingEffectId;
        RemoveEffect(id);

        Debug.Log("removing effect index " + index + " id " + id);
        DebugPrintList();
    }

    public void RemoveEffect(int id) {
        effects.RemoveAll(x => x.id == id);
        GameManager.Instance.hero.RecalculateStats();
    }

    public void ApplyEffects() {
        Debug.Log("applying");
        foreach (Effect effect in effects) {
            effect.Apply();
        }
        effects.RemoveAll(x => x.duration == 0);
        GameManager.Instance.hero.RecalculateStats();
        UIManager.Instance.RefreshHUD();
    }

    public void CalculateEffects() {
        foreach (Effect effect in effects) {
            effect.Calculate();
        }
    }

}

public class Effect {

    public EffectName name;
    public int duration;
    public bool isDebuff;
    
    public StatType stat;
    public int value;
    public int id;

    public void Calculate() {
        if(stat == StatType.CurrHP) {
            // only when applying
        } else {
            if (isDebuff) {
                GameManager.Instance.hero.IncreaseStatTemporary(-value, stat);
            } else {
                GameManager.Instance.hero.IncreaseStatTemporary(value, stat);
            }
        }
    }

    public void Apply() {
        Calculate();
        if(stat == StatType.CurrHP) {
            if (isDebuff) {
                GameManager.Instance.hero.TakePoisonDmg(value);
            } else {
                GameManager.Instance.hero.HealBuff(value);
            }
        }
        if(duration > 0)
            duration--;
    }

    public Effect(EffectName effectName, int duration, int value, int id) {
        if (EffectDB.effects.TryGetValue(effectName, out var data)) {
            name = effectName;
            this.duration = duration;
            isDebuff = data.isDebuff;
            stat = data.stat;
            this.value = value;
            this.id = id;
        } else {
            Debug.LogError($"Effect '{effectName}' not found in database!");
        }
    }

}