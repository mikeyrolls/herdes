
using System.Collections.Generic;
using UnityEngine;

public class EffectList {
    public List<Effect> effects = new List<Effect> {};

    public void RemoveDebuffs() {
        effects.RemoveAll(x => x.isDebuff);
    }

    public void AddEffect(EffectName effectName) {
        effects.RemoveAll(x => x.name == effectName);
        Effect effect = new Effect();
        effect.InitializeFromDB(effectName);
        effects.Add(effect);
        GameManager.Instance.hero.RecalculateStats();
    }

    public void AddEffectDurationValue(EffectName effectName, int duration, int value) {
        effects.RemoveAll(x => x.name == effectName);
        Effect effect = new Effect();
        effect.InitNameDurationValue(effectName, duration, value);
        effects.Add(effect);
        GameManager.Instance.hero.RecalculateStats();
    }

    public void ApplyEffects() {
        Debug.Log("applying");
        foreach (Effect effect in effects) {
            effect.Apply();
        }
        effects.RemoveAll(x => x.duration <= 0);
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
        duration--;
    }

    public void InitializeFromDB(EffectName effectName) {
        if (EffectDB.effects.TryGetValue(effectName, out var data)) {
            name = effectName;
            duration = data.duration;
            isDebuff = data.isDebuff;
            stat = data.stat;
            value = data.value;
        } else {
            Debug.LogError($"Effect '{effectName}' not found in database!");
        }
    }

    public void InitNameDurationValue(EffectName effectName, int duration, int value) {
        if (EffectDB.effects.TryGetValue(effectName, out var data)) {
            name = effectName;
            this.duration = duration;
            isDebuff = data.isDebuff;
            stat = data.stat;
            this.value = value;
        } else {
            Debug.LogError($"Effect '{effectName}' not found in database!");
        }
    }

}